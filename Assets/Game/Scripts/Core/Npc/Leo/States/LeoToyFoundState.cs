using Game.Scripts.Core.NPC.Leo;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoToyFoundState : LeoBaseState
    {
        public LeoToyFoundState(LeoTutorial context) : base(context, LeoState.ToyFound) { }

        public override void Enter()
        {
            _navigator.MoveTo(_context.ToyPoint);
            
            _interactable.OnInteractionProceed.AddListener(OnInteract);
        }

        protected override void OnInteract()
        {
            var dialogue = _context.ToyFoundDialogue;
            
            dialogue.OnDialogueFinish.AddListener(OnDialogueComplete);
            dialogue.StartDialogue();
            
            _interactable.OnInteractionProceed.RemoveListener(OnInteract);
        }
        
        private void OnDialogueComplete()
        {
            _context.ToyFoundDialogue.OnDialogueFinish.RemoveListener(OnDialogueComplete);
            _context.SetState(LeoState.Finished);
        }
    }
}