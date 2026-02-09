using Game.Scripts.Core.NPC.Leo;
using UnityEngine;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoWaitNearBalconyState : LeoBaseState
    {
        public LeoWaitNearBalconyState(LeoTutorial context) : base(context, LeoState.WaitNearBalcony) { }

        public override void Enter()
        {
            _navigator.MoveTo(_context.BalconyPoint.position, OnReachedBalcony);
        }

        protected override void OnInteract()
        {
            if (_navigator.IsMoving)
            {
                Debug.Log("Trying to interact with moving leo");
                return;
            }

            var dialogue = _context.BalconyDialogue;
            
            dialogue.OnDialogueFinish.AddListener(OnDialogueComplete);
            dialogue.StartDialogue();
        }

        public override void Exit()
        {
            _interactable.OnInteractionProceed.RemoveListener(OnInteract);
        }

        private void OnReachedBalcony()
        {
            _navigator.SetLookTarget(_context.BalconyPoint);
            _interactable.OnInteractionProceed.AddListener(OnInteract);
        }

        private void OnDialogueComplete()
        {
            _context.BalconyDialogue.OnDialogueFinish.RemoveListener(OnDialogueComplete);
        }
    }
}