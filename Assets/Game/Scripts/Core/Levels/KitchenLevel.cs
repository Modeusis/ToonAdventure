using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Npc.Adam;
using Game.Scripts.Core.Triggers;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Levels
{
    public class KitchenLevel : Level
    {
        [Header("Adam")]
        [SerializeField] private AdamKitchen _adamNpc;
        
        [Header("Cafeteria")]
        [SerializeField] private InteractableObject _toy;
        [SerializeField] private LockedTrigger _cafeteriaDoorBlocker;
        
        [Header("Puzzle")]
        [SerializeField] private InteractableObject _puzzleInteractable;
        
        [Header("Exit")]
        [SerializeField] private LockedTrigger _exitDoorBlocker;
        [SerializeField] private LevelType _nextLevelType;

        public override void Load()
        {
            base.Load();
            
            _exitDoorBlocker.OnEnter.AddListener(LoadNextLevel);
            G.EventBus.Subscribe<OnQuestCompletedEvent>(HandleQuestFinished);
        }

        public override void Unload()
        {
            _exitDoorBlocker.OnEnter.RemoveListener(LoadNextLevel);
            G.EventBus.Unsubscribe<OnQuestCompletedEvent>(HandleQuestFinished);
            
            base.Unload();
        }

        private void HandleQuestFinished(OnQuestCompletedEvent eventData)
        {
            if (eventData.QuestId != QuestId)
                return;
            
            _exitDoorBlocker.Unlock();
        }

        private void LoadNextLevel()
        {
            G.EventBus.Publish(_nextLevelType);
        }
    }
}