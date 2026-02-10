using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Interactions.Fridge;
using Game.Scripts.Core.Interactions.Pickups;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.Core.Npc.Adam;
using Game.Scripts.Core.Puzzle;
using Game.Scripts.Core.Triggers;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Levels
{
    public class KitchenLevel : Level
    {
        [Header("Adam")]
        [SerializeField] private AdamKitchen _adamNpc;
        [SerializeField] private LockedTrigger _fridgeLocker;
        
        [Header("Kitchen Puzzle")]
        [SerializeField] private FridgeInteractable _fridgeInteractable;
        [SerializeField] private Door _fridgeDoor;
        [SerializeField] private LightPuzzle _fridgePuzzle;
        [SerializeField] private Pickup _fridgePickup;
        
        [Header("Cafeteria")]
        [SerializeField] private Pickup _cafeteriaPickup;
        [SerializeField] private InteractableObject _cafeteriaInteractable;
        [SerializeField] private LockedTrigger _cafeteriaDoorBlocker;
        
        [Header("Exit")]
        [SerializeField] private LockedTrigger _exitDoorBlocker;
        [SerializeField] private LevelType _nextLevelType;
        
        [Header("Quest Info")]
        [SerializeField] private string _adamTalkName = "talk_adam";
        [SerializeField] private string _fridgeToyName = "toy_cactus";
        [SerializeField] private string _cafeteriaToyName = "toy_keys";
        
        [Header("Dialogues")]
        [SerializeField] private Dialogue _fridgeMonologue;
        [SerializeField] private Dialogue _fridgeToyFoundMonologue;
        [SerializeField] private Dialogue _finishedMonologue;

        public override void Load()
        {
            base.Load();
            
            SubscribeToEvents();
            InitializeLevelState();
        }

        public override void Unload()
        {
            UnsubscribeFromEvents();
            base.Unload();
        }

        private void SubscribeToEvents()
        {
            G.EventBus.Subscribe<OnQuestCompletedEvent>(OnQuestCompleted);
            G.EventBus.Subscribe<OnPuzzleSolvedEvent>(OnPuzzleSolved);
            
            _adamNpc.DialogStart.OnDialogueFinish.AddListener(ActivateFridge);
            _fridgeMonologue.OnDialogueFinish.AddListener(StartFridgePuzzle);
            
            _exitDoorBlocker.OnEnter.AddListener(LoadNextLevel);
            
            _fridgeInteractable.AddListenerToState(FridgeState.Locked, _fridgeMonologue.StartDialogue);
            _fridgeInteractable.AddListenerToState(FridgeState.Closed, OpenFridge);
            _fridgeInteractable.AddListenerToState(FridgeState.Opened, CollectFridgeToy);
            _fridgeInteractable.AddListenerToState(FridgeState.PickUpCollected, ToggleFridgeDoor);
            
            _cafeteriaInteractable.OnInteractionProceed.AddListener(CollectCafeteriaToy);
        }

        private void UnsubscribeFromEvents()
        {
            G.EventBus.Unsubscribe<OnQuestCompletedEvent>(OnQuestCompleted);
            G.EventBus.Unsubscribe<OnPuzzleSolvedEvent>(OnPuzzleSolved);

            _adamNpc.DialogStart.OnDialogueFinish.RemoveListener(ActivateFridge);
            _fridgeMonologue.OnDialogueFinish.RemoveListener(StartFridgePuzzle);

            _exitDoorBlocker.OnEnter.RemoveListener(LoadNextLevel);

            _fridgeInteractable.RemoveListenerFromState(FridgeState.Locked, _fridgeMonologue.StartDialogue);
            _fridgeInteractable.RemoveListenerFromState(FridgeState.Closed, OpenFridge);
            _fridgeInteractable.RemoveListenerFromState(FridgeState.Opened, CollectFridgeToy);
            _fridgeInteractable.RemoveListenerFromState(FridgeState.PickUpCollected, ToggleFridgeDoor);

            _cafeteriaInteractable.OnInteractionProceed.RemoveListener(CollectCafeteriaToy);
            
            _fridgePuzzle.StopPuzzle();
        }

        private void InitializeLevelState()
        {
            _fridgeInteractable.SetState(FridgeState.Locked);
        }
        
        private void StartFridgePuzzle()
        {
            _fridgePuzzle.BeginPuzzle();
        }

        private void OnPuzzleSolved(OnPuzzleSolvedEvent eventData)
        {
            _fridgeInteractable.SetState(FridgeState.Closed);
            _fridgeMonologue.OnDialogueFinish.RemoveListener(StartFridgePuzzle);
        }

        private void ActivateFridge()
        {
            _fridgeLocker.Unlock();
            
            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _adamTalkName,
                Amount = 1 
            });
            
            _adamNpc.DialogStart.OnDialogueFinish.RemoveListener(ActivateFridge);
        }
        
        private void OpenFridge()
        {
            _fridgeDoor.Toggle();
            _fridgeInteractable.SetState(FridgeState.Opened);
        }

        private void CollectFridgeToy()
        {
            if (_fridgePickup != null)
                _fridgePickup.PickupObject();

            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _fridgeToyName, 
                Amount = 1 
            });

            _adamNpc.CurrentState++;
            
            _fridgeInteractable.SetState(FridgeState.PickUpCollected);
            _cafeteriaDoorBlocker.Unlock();
            _fridgeToyFoundMonologue.StartDialogue();
        }

        private void CollectCafeteriaToy()
        {
            if (_cafeteriaPickup != null)
                _cafeteriaPickup.PickupObject();
            
            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _cafeteriaToyName, 
                Amount = 1 
            });
                
            _adamNpc.CurrentState++;
            
            _exitDoorBlocker.Unlock();
            _finishedMonologue.StartDialogue();
        }

        private void ToggleFridgeDoor()
        {
            _fridgeDoor.Toggle();
        }

        private void OnQuestCompleted(OnQuestCompletedEvent eventData)
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