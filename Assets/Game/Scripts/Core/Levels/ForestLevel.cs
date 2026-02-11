using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Cutscene;
using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Interactions.Pickups;
using Game.Scripts.Core.Interactions.Tree;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.Core.NPC.Maks;
using Game.Scripts.Core.Triggers;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Levels
{
    public class ForestLevel : Level
    {
        [Header("NPC & Hub")]
        [SerializeField] private MaksYard _maksimNpc;
        [SerializeField] private Dialogue _maksimStartDialogue;
        [SerializeField] private Dialogue _maksimBaseDialogue;
        [SerializeField] private Transform _runAwayPoint;
        [SerializeField] private DialogueTrigger _maksMissingDialogueTrigger;
        
        [Header("Tree Area")]
        [SerializeField] private TreeInteractable _treeInteractable;
        [SerializeField] private LockedTrigger _treeLocker;
        [SerializeField] private ParticleSystem _digEffect;
        
        [SerializeField, Space] private Pickup _treePickupPrefab;
        [SerializeField] private Transform _treePickupSpawnPoint;
        
        [SerializeField, Space] private Dialogue _treeToyFirstDialogue; 
        [SerializeField] private Dialogue _treeToyCollectDialogue; 
        [SerializeField] private Dialogue _treeToyCollectedDialogue; 
        
        [Header("Rock Area")]
        [SerializeField] private InteractableObject _rockInteractable;
        [SerializeField] private LockedTrigger _rockLocker;
        [SerializeField] private Pickup _rockPickup;
        
        [SerializeField, Space] private Dialogue _rockToyDialogue;
        [SerializeField] private Dialogue _leoFoundDialogue;
        
        [Header("Final Zone (Mushrooms)")]
        [SerializeField] private LockedTrigger _mushroomPathBlocker;
        
        [Header("Quest IDs")]
        [SerializeField] private string _maksimTalkId = "talk_maks";
        [SerializeField] private string _toyFoundId = "toy_found";
        [SerializeField] private string _leoFoundId = "find_leo";

        private Pickup _treePickup;
        
        public override void Load()
        {
            base.Load();
            
            SubscribeToEvents();
        }

        public override void Unload()
        {
            UnsubscribeFromEvents();
            base.Unload();
        }

        private void SubscribeToEvents()
        {
            G.EventBus.Subscribe<OnQuestStepCompletedEvent>(HandleToysFound);
            G.EventBus.Subscribe<OnQuestCompletedEvent>(FinalDialogue);
            
            _maksimStartDialogue.OnDialogueFinish.AddListener(OnMaksimFirstInteraction);
            
            _treeToyFirstDialogue.OnDialogueFinish.AddListener(OnTreeFirstInteraction);
            _treeToyCollectDialogue.OnDialogueFinish.AddListener(CollectTreeToy);
            
            _rockToyDialogue.OnDialogueFinish.AddListener(CollectRockToy);
            
            _leoFoundDialogue.OnDialogueFinish.AddListener(LoadCutscene);
            
            _maksimNpc.AddListenerToState(MaksState.FirstInteraction, _maksimStartDialogue.StartDialogue);
            _maksimNpc.AddListenerToState(MaksState.Base, _maksimBaseDialogue.StartDialogue);
            
            _maksimNpc.OnInteractionZoneEnter.AddListener(OnMaksimInteractionEnter);
            _maksimNpc.OnInteractionZoneExit.AddListener(_maksimNpc.Navigator.ClearLookTarget);
            _maksimNpc.OnInteractionProceed.AddListener(_maksimNpc.AnimationController.PlayAction);
            
            _treeInteractable.AddListenerToState(TreeState.FirstInteraction, _treeToyFirstDialogue.StartDialogue);
            _treeInteractable.AddListenerToState(TreeState.DigOut, _treeToyCollectDialogue.StartDialogue);
            _treeInteractable.AddListenerToState(TreeState.ToyPickedUp, _treeToyCollectedDialogue.StartDialogue);
            
            _rockInteractable.OnInteractionProceed.AddListener(_rockToyDialogue.StartDialogue);
            
            _mushroomPathBlocker.OnEnter.AddListener(FindLeo);
        }

        private void UnsubscribeFromEvents()
        {
            G.EventBus.Unsubscribe<OnQuestStepCompletedEvent>(HandleToysFound);
            G.EventBus.Unsubscribe<OnQuestCompletedEvent>(FinalDialogue);
            
            _maksimStartDialogue.OnDialogueFinish.RemoveListener(OnMaksimFirstInteraction);
            
            _treeToyFirstDialogue.OnDialogueFinish.RemoveListener(OnTreeFirstInteraction);
            _treeToyCollectDialogue.OnDialogueFinish.RemoveListener(CollectTreeToy);
            
            _rockToyDialogue.OnDialogueFinish.RemoveListener(CollectRockToy);
            
            _leoFoundDialogue.OnDialogueFinish.RemoveListener(LoadCutscene);
            
            _maksimNpc.RemoveListenerFromState(MaksState.FirstInteraction, _maksimStartDialogue.StartDialogue);
            _maksimNpc.RemoveListenerFromState(MaksState.Base, _maksimBaseDialogue.StartDialogue);
            
            _maksimNpc.OnInteractionZoneEnter.RemoveListener(OnMaksimInteractionEnter);
            _maksimNpc.OnInteractionZoneExit.RemoveListener(_maksimNpc.Navigator.ClearLookTarget);
            _maksimNpc.OnInteractionProceed.RemoveListener(_maksimNpc.AnimationController.PlayAction);
            
            _treeInteractable.RemoveListenerFromState(TreeState.FirstInteraction, _treeToyFirstDialogue.StartDialogue);
            _treeInteractable.RemoveListenerFromState(TreeState.DigOut, _treeToyCollectDialogue.StartDialogue);
            _treeInteractable.RemoveListenerFromState(TreeState.ToyPickedUp, _treeToyCollectedDialogue.StartDialogue);
            
            _rockInteractable.OnInteractionProceed.RemoveListener(_rockToyDialogue.StartDialogue);
            
            _mushroomPathBlocker.OnEnter.RemoveListener(FindLeo);
        }

        private void OnMaksimInteractionEnter(Transform playerTransform)
        {
            _maksimNpc.Navigator.SetLookTarget(playerTransform);
            _maksimNpc.AnimationController.PlayAction();
        }
        
        private void OnMaksimFirstInteraction()
        {
            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _maksimTalkId,
                Amount = 1 
            });
            
            _maksimStartDialogue.OnDialogueFinish.RemoveListener(OnMaksimFirstInteraction);
            _maksimNpc.SetState(MaksState.Base);
            
            _rockLocker.Unlock();
            _treeLocker.Unlock();
        }

        private void HandleToysFound(OnQuestStepCompletedEvent eventData)
        {
            if (eventData.TargetId != _toyFoundId)
                return;
            
            _mushroomPathBlocker.Unlock();
            
            _maksMissingDialogueTrigger.gameObject.SetActive(true);
            
            _maksimNpc.OnInteractionZoneEnter.RemoveListener(OnMaksimInteractionEnter);
            
            _maksimNpc.SetState(MaksState.AllToysFound, showNewTag: false);
            
            _maksimNpc.Navigator.MoveTo(_runAwayPoint.position, () =>
            {
                _maksimNpc.gameObject.SetActive(false);
            });
        }

        private void OnTreeFirstInteraction()
        {
            _treeToyFirstDialogue.OnDialogueFinish.RemoveListener(OnTreeFirstInteraction);
            
            _digEffect?.Play();
            
            _treePickup = Instantiate(_treePickupPrefab, _treePickupSpawnPoint.position, _treePickupSpawnPoint.rotation);
            _treeInteractable.SetState(TreeState.DigOut);
        }

        private void CollectTreeToy()
        {
            if (_treePickup != null)
                _treePickup.PickupObject();

            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _toyFoundId, 
                Amount = 1 
            });
            
            _treeToyCollectDialogue.OnDialogueFinish.RemoveListener(CollectTreeToy);
            _treeInteractable.SetState(TreeState.ToyPickedUp);
        }
        
        private void CollectRockToy()
        {
            if (_rockPickup != null)
                _rockPickup.PickupObject();
            
            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _toyFoundId, 
                Amount = 1 
            });
            
            _rockToyDialogue.OnDialogueFinish.RemoveListener(CollectRockToy);
        }

        private void FindLeo()
        {
            G.EventBus.Publish(new OnQuestProgressEvent 
            { 
                TargetId = _leoFoundId, 
                Amount = 1 
            });
        }

        private void LoadCutscene()
        {
            _leoFoundDialogue.OnDialogueFinish.RemoveListener(LoadCutscene);
            G.Scenes.LoadCutscene().Forget();
        }
        
        private void FinalDialogue(OnQuestCompletedEvent eventData)
        {
            if (eventData.QuestId != QuestId)
                return;
            
            _leoFoundDialogue.StartDialogue();
        }
    }
}