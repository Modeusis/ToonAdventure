using Cysharp.Threading.Tasks;
using Game.Scripts.Setups.Quests;
using Game.Scripts.UI.Screens.Quest;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Loop
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField, Space] private QuestLineData _questLineData;
        [SerializeField, Space] private QuestView _questViewPrefab;
        
        private QuestData _activeQuest;
        private QuestView _view;
        
        private bool _isInitialized;

#if UNITY_EDITOR
        private async void Start()
        {
            await UniTask.WaitUntil(() => G.IsReady);
            
            if (!G.IsTestMode) return;

            Initialize();
        }
#endif

        public void Initialize()
        {
            if (_isInitialized) 
                return;

            var screensTransform = G.UI.Screens.transform;
            _view = Instantiate(_questViewPrefab, screensTransform);
            
            G.EventBus.Subscribe<OnQuestProgressEvent>(OnQuestProgress);
            
            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (!G.IsReady) 
                return;
            
            if (_activeQuest) Destroy(_activeQuest);
            if (_view) Destroy(_view.gameObject);
            
            G.EventBus.Unsubscribe<OnQuestProgressEvent>(OnQuestProgress);
        }

        public void StartQuest(string questId)
        {
            if (_activeQuest)
            {
                Destroy(_activeQuest);
                _activeQuest = null;
            }
            
            var quest = _questLineData.GetQuestById(questId);
            if (!quest)
            {
                Debug.Log("[QuestManager.StartQuest] No more quests.");
                _view.Hide();
                return;
            }

            _activeQuest = Instantiate(quest); 
            _activeQuest.ResetProgress();
            
            _view.Show(_activeQuest);
            
            Debug.Log($"[QuestManager.StartQuest] Started quest: {_activeQuest.Title}");
        }

        private void OnQuestProgress(OnQuestProgressEvent eventData)
        {
            if (!_activeQuest)
                return;

            var questUpdated = false;

            foreach (var step in _activeQuest.Steps)
            {
                if (step.IsCompleted)
                    continue;

                if (step.TargetId != eventData.TargetId)
                    continue;
                
                step.CurrentAmount += eventData.Amount;
                    
                if (step.CurrentAmount >= step.RequiredAmount)
                {
                    step.CurrentAmount = step.RequiredAmount;
                    step.IsCompleted = true;
                    
                    G.EventBus.Publish(new OnQuestStepCompletedEvent { TargetId = step.TargetId, StepDescription = step.Description });
                    
                    Debug.Log($"[QuestManager.StartQuest] Completed quest step: {step.Description}");
                }
                    
                questUpdated = true;
            }

            if (!questUpdated) 
                return;
            
            _view.Refresh(_activeQuest);
            
            CheckQuestCompletion();
        }

        private void CheckQuestCompletion()
        {
            var allStepsDone = true;
            foreach (var step in _activeQuest.Steps)
            {
                if (step.IsCompleted)
                    continue;
                
                allStepsDone = false;
                break;
            }

            if (allStepsDone)
            {
                CompleteCurrentQuest();
            }
        }

        private void CompleteCurrentQuest()
        {
            Debug.Log($"[QuestManager.CompleteCurrentQuest] Completed quest: {_activeQuest.Title}");
            
            G.EventBus.Publish(new OnQuestCompletedEvent { QuestId = _activeQuest.Id });

            foreach (var action in _activeQuest.ActionsOnComplete)
            {
                action.Execute();
            }
        }
    }
}