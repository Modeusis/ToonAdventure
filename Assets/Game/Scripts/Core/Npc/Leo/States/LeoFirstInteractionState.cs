using Game.Scripts.Core.NPC.Leo;
using UnityEngine;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoFirstInteractionState : LeoBaseState
    {
        public LeoFirstInteractionState(LeoTutorial context) : base(context, LeoState.FirstInteraction) { }
        
        public override void Enter()
        {
            _navigator.Stop();
            
            _interactable.OnInteractionProceed.AddListener(OnInteract);
            _interactable.OnInteractionZoneEnter.AddListener(DogInteractionEnter);
            _interactable.OnInteractionZoneExit.AddListener(() => _navigator.SetLookTarget(null));
        }

        protected override void OnInteract()
        {
            var dialogue = _context.GreetingDialogue;
            
            dialogue.OnDialogueFinish.AddListener(OnDialogueComplete);
            dialogue.StartDialogue();
            
            _interactable.OnInteractionProceed.RemoveListener(OnInteract);
        }

        private void DogInteractionEnter(Transform transform)
        {
            _navigator.SetLookTarget(transform);
            _animator.PlayAction();
        }
        
        private void OnDialogueComplete()
        {
            _context.GreetingDialogue.OnDialogueFinish.RemoveListener(OnDialogueComplete);
            _context.SetState(LeoState.WaitNearBalcony);
        }
    }
}