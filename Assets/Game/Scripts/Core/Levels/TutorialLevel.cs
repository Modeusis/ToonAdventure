using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.NPC.Leo;
using Game.Scripts.Core.Triggers;
using Game.Scripts.Data;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Levels
{
    public class TutorialLevel : Level
    {
        [Header("General")]
        [SerializeField] private LevelType _nextLevelType;
        
        [Header("Leo settings")]
        [SerializeField, Space] private Transform _leoSpawnPoint;
        [SerializeField] private LeoTutorial _leoPrefab;

        [SerializeField, Space] private Transform _leoBalconyPoint;
        [SerializeField] private Transform _leoToyFoundPoint;
        
        [Header("Level triggers")]
        [SerializeField, Space] private LockedTrigger _nextLevelTrigger;
        [SerializeField] private LockedTrigger _balconyLevelTrigger;
        
        [Header("Toy pickup settings")]
        [SerializeField] private InteractableObject _toyPickup;
        
        [Header("Quest Info")]
        [SerializeField] private string _firstInteractionStepName = "talk_leo";
        [SerializeField] private string _findToyStepName = "toy_duck";
        
        private LeoTutorial _leo;
        
        public override void Load()
        {
            base.Load();
            
            _leo = Instantiate(_leoPrefab, _leoSpawnPoint.position, _leoSpawnPoint.rotation);
            _leo.transform.SetParent(transform);
            
            var leoData = new LeoTutorialData(_leoBalconyPoint, _leoToyFoundPoint);
            
            _leo.GreetingDialogue.OnDialogueFinish.AddListener(FinishFirstInteractionStep);
            _toyPickup.OnInteractionProceed.AddListener(FinishTutorial);
            
            _nextLevelTrigger.OnEnter.AddListener(LoadNextLevel);
            
            _leo.Initialize(leoData);
        }

        public override void Unload()
        {
            _leo.GreetingDialogue.OnDialogueFinish.RemoveAllListeners();
            _toyPickup.OnInteractionProceed.RemoveAllListeners();
            
            _nextLevelTrigger.OnEnter.RemoveListener(LoadNextLevel);
            
            Destroy(_leo.gameObject);
            
            base.Unload();
        }

        private void FinishFirstInteractionStep()
        {
            G.EventBus.Publish(new OnQuestProgressEvent { Amount = 1, TargetId = _firstInteractionStepName});
            _balconyLevelTrigger.Unlock();
        }
        
        private void FinishTutorial()
        {
            G.EventBus.Publish(new OnQuestProgressEvent { Amount = 1, TargetId = _findToyStepName});
            _nextLevelTrigger.Unlock();
        }

        private void LoadNextLevel()
        {
            G.EventBus.Publish(_nextLevelType);
        }
    }
}