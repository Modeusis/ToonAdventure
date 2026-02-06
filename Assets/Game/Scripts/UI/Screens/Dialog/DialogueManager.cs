using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Setups.Core;
using Game.Scripts.Utilities.Constants;
using Game.Scripts.Utilities.Events;
using Ink.Runtime;
using UnityEngine;

namespace Game.Scripts.UI.Screens.Dialog
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private DialogueCharacterDatabase _characterDatabase;
        [SerializeField] private DialogueView _viewPrefab;
        
        private const string SPEAKER_TAG = "speaker";
        
        private DialogueView _view;
        private DialogueCharacterDatabase _database;
        
        private Story _currentStory;

        public bool IsInitialized { get; private set; }

        public bool IsPlaying => _currentStory != null;

#if UNITY_EDITOR
        private async void Start()
        {
            await UniTask.WaitUntil(() => G.IsReady);
            
            if (!G.IsTestMode)
                return;

            Initialize();
        }
#endif
        
        public void Initialize()
        {
            if (IsInitialized)
                return;

            var screenTransform = G.UI.Screens.transform;

            _view = Instantiate(_viewPrefab, screenTransform);
            _view.transform.position = Vector3.zero;
            _view.name = "Dialogue";
            
            _view.SetVisible(false);
            
            G.EventBus.Subscribe<StartDialogueRequestEvent>(OnStartDialogueRequested);
            
            IsInitialized = true;
        }

        private void OnDestroy()
        {
            if (!G.IsReady)
                return;
            
            G.EventBus.Unsubscribe<StartDialogueRequestEvent>(OnStartDialogueRequested);
        }

        private void OnStartDialogueRequested(StartDialogueRequestEvent request)
        {
            if (!request.InkAsset)
                return;
            
            StartDialogue(request.InkAsset, request.StartPath);
        }

        private void OnContinueDialogueRequested(ContinueDialogueRequestEvent request)
        {
            if (!_currentStory)
                return;
            
            Continue();
        }
        
        private void StartDialogue(TextAsset inkAsset, string startPath = "")
        {
            if (inkAsset == null)
            {
                Debug.LogError("[DialogueManager] Ink asset is null!");
                return;
            }

            _currentStory = new Story(inkAsset.text);
            
            if (!string.IsNullOrEmpty(startPath))
            {
                try
                {
                    _currentStory.ChoosePathString(startPath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[DialogueSystem] Ошибка перехода к узлу '{startPath}': {e.Message}");
                }
            }
            
            _view.SetVisible(true);
            
            G.EventBus.Publish(new OnDialogueStateChangedEvent{ IsActive = true });
            
            RefreshView();
        }

        private void End()
        {
            _currentStory = null;
            _view.SetVisible(false);
            _view.ClearChoices();
            
            G.EventBus.Publish(new OnDialogueStateChangedEvent{ IsActive = false });
        }

        private void Continue()
        {
            if (_currentStory.canContinue)
            {
                RefreshView();
            }
            else
            {
                End();
            }
        }

        private void RefreshView()
        {
            string text = "";
            
            if (_currentStory.canContinue)
            {
                text = _currentStory.Continue();
            }

            ParseTags(_currentStory.currentTags, out string name, out Sprite portrait);
            
            _view.SetContent(name, text, portrait);
            _view.ClearChoices();

            if (_currentStory.currentChoices.Count > 0)
            {
                foreach (var choice in _currentStory.currentChoices)
                {
                    _view.CreateChoice(choice.text, () => MakeChoice(choice.index));
                }
            }
            else
            {
                
            }
        }

        private void MakeChoice(int choiceIndex)
        {
            _currentStory.ChooseChoiceIndex(choiceIndex);
            RefreshView();
        }

        private void ParseTags(List<string> tags, out string name, out Sprite portrait)
        {
            name = "???";
            portrait = null;
            string characterId = "";

            foreach (var tag in tags)
            {
                var parts = tag.Split(':');
                if (parts.Length != 2) continue;
                
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (key == SPEAKER_TAG) characterId = value;
            }

            if (!string.IsNullOrEmpty(characterId) && _database != null)
            {
                if (_database.TryGetCharacter(characterId, out var data))
                {
                    name = data.DisplayName;
                    portrait = data.DefaultPortrait;
                }
                else
                {
                    name = characterId; 
                }
            }
        }

        public void Dispose()
        {
            _currentStory = null;
        }
    }
}