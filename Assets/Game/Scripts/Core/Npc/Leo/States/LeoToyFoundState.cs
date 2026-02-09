using Game.Scripts.Core.NPC.Leo;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoToyFoundState : LeoBaseState
    {
        public LeoToyFoundState(LeoTutorial context) : base(context, LeoState.ToyFound) { }

        public override void Enter()
        {
            if (_context.ToyPoint != null)
            {
                _navigator.MoveTo(_context.ToyPoint);
            }
        }

        protected override void OnInteract()
        {
            var dialogue = _context.ToyFoundDialogue;
            
            dialogue.OnDialogueFinish.AddListener(OnDialogueComplete);
            dialogue.StartDialogue();
        }
        
        private void OnDialogueComplete()
        {
            _context.ToyFoundDialogue.OnDialogueFinish.RemoveListener(OnDialogueComplete);
            _context.SetState(LeoState.Finished);
        }
    }
}