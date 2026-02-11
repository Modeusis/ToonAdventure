using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Cutscene.CutsceneStates;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.StateMachine;
using UnityEngine;

namespace Game.Scripts.Core.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private Dialogue _cutsceneDialogue;
        [SerializeField] private CutsceneActors _actors;
        
        [Header("Configuration")]
        [SerializeField] private List<CutsceneConfig> _configs;

        private FSM<CutsceneState> _fsm;
        
        private Dictionary<CutsceneState, State<CutsceneState>> _createdStates;        
        
        private bool _isPlaying;
        private CutsceneState _requestedState = CutsceneState.None;

        private async void Start()
        {
            await UniTask.WaitUntil(() => G.IsReady);
            
            if (this == null) return;
            
            _dialogueManager.Initialize();
            
            InitializeFSM();
            
            foreach (var config in _configs)
            {
                if (config.VirtualCamera != null)
                    config.VirtualCamera.gameObject.SetActive(false);
            }
            
            G.EventBus.Subscribe<OnTagResievedEvent>(HandleTag);
            
            StartCutscene();
        }
        
        private void OnDestroy()
        {
            if (G.IsReady)
            {
                G.EventBus.Unsubscribe<OnTagResievedEvent>(HandleTag);
            }
            
            if (_createdStates != null)
            {
                foreach (var stateKvp in _createdStates)
                {
                    if (stateKvp.Value is IDisposable disposableState)
                    {
                        disposableState.Dispose();
                    }
                }
                _createdStates.Clear();
            }
        }

        public void StartCutscene()
        {
            _isPlaying = true;
            _cutsceneDialogue.StartDialogue();
        }

        private void InitializeFSM()
        {
            _createdStates = new Dictionary<CutsceneState, State<CutsceneState>>();
            var transitions = new List<Transition<CutsceneState>>();

            var noneConfig = new CutsceneConfig { State = CutsceneState.None };
            
            _createdStates.Add(CutsceneState.None, new NoneState(noneConfig, _actors, null));
            transitions.Add(new Transition<CutsceneState>(CutsceneState.None, () => _requestedState == CutsceneState.None));
            
            foreach (var config in _configs)
            {
                Debug.Log("State: " + config.State);
                
                if (config.State == CutsceneState.None)
                    continue;
                
                CutsceneBaseState state;
                Action onStateComplete = OnStateCompleted;

                switch (config.State)
                {
                    case CutsceneState.Intro:
                        state = new IntroState(config, _actors, onStateComplete);
                        break;
                    case CutsceneState.Surprise:
                        state = new SurpriseState(config, _actors, onStateComplete);
                        break;
                    case CutsceneState.CharlieReaction:
                        state = new CharlieReactionState(config, _actors, onStateComplete);
                        break;
                    case CutsceneState.LeoHappy:
                        state = new LeoHappyState(config, _actors, onStateComplete);
                        break;
                    case CutsceneState.Outro:
                        state = new OutroState(config, _actors, onStateComplete, OnCutsceneFinish);
                        break;
                    default:
                        state = new NoneState(config, _actors, onStateComplete);
                        break;
                }

                _createdStates.Add(config.State, state);
                transitions.Add(new Transition<CutsceneState>(config.State, () => _requestedState == config.State));
            }
            
            _fsm = new FSM<CutsceneState>(_createdStates, transitions, CutsceneState.None);
        }

        private void Update()
        {
            if (!_isPlaying)
                return;
            
            _fsm?.Update();
        }

        private void LateUpdate()
        {
            _fsm?.LateUpdate();
        }

        private void HandleTag(OnTagResievedEvent eventData)
        {
            if (eventData.Key != "cutscene")
                return;
            
            if (Enum.TryParse(eventData.Value, true, out CutsceneState newState))
            {
                _requestedState = newState;
            }
            else
            {
                Debug.LogWarning($"[CutsceneManager] Unknown cutscene state: {eventData.Value}");
            }
        }

        private void OnStateCompleted()
        {
            if (_requestedState != CutsceneState.Outro)
            {
                _requestedState = CutsceneState.None;
                G.EventBus.Publish(new ContinueDialogueRequestEvent());
            }
        }

        private void OnCutsceneFinish()
        {
            _isPlaying = false;
            G.EventBus.Publish(new ContinueDialogueRequestEvent());
            G.UI.PopUp.Final.Show();
        }
    }
}