using Cysharp.Threading.Tasks;
using Game.Scripts.Setups.Quests;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Loop
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestLineData _questLineData;
        
        private int _currentQuestIndex = 0;
        private QuestData _activeQuest;
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
            if (_isInitialized) return;

            G.EventBus.Subscribe<OnQuestProgressEvent>(OnQuestProgress);
            
            StartQuest(_currentQuestIndex);
            
            _isInitialized = true;
            Debug.Log("[QuestManager.Initialize] Initialized");
        }

        private void OnDestroy()
        {
            if (!G.IsReady) return;
            
            G.EventBus.Unsubscribe<OnQuestProgressEvent>(OnQuestProgress);
        }

        private void StartQuest(int index)
        {
            var quest = _questLineData.GetQuestByIndex(index);
            if (!quest)
            {
                Debug.Log("[QuestManager.StartQuest] No more quests.");
                return;
            }

            _activeQuest = Instantiate(quest); 
            _activeQuest.ResetProgress();
            _currentQuestIndex = index;
            
            Debug.Log($"[QuestManager.StartQuest] Started Quest: {_activeQuest.Title}");
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
                    G.EventBus.Publish(new OnQuestStepCompletedEvent { StepDescription = step.Description });
                }
                    
                questUpdated = true;
            }

            if (questUpdated)
            {
                CheckQuestCompletion();
            }
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
            Debug.Log($"[QuestManager.CompleteCurrentQuest] Completed Quest: {_activeQuest.Title}");
            
            G.EventBus.Publish(new OnQuestCompletedEvent { QuestId = _activeQuest.Id });

            foreach (var action in _activeQuest.ActionsOnComplete)
            {
                action.Execute();
            }

            StartQuest(_currentQuestIndex + 1);
        }
    }
}