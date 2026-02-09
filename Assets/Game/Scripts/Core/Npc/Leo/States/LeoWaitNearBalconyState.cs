using Game.Scripts.Core.NPC.Leo;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoWaitNearBalconyState : LeoBaseState
    {
        public LeoWaitNearBalconyState(LeoTutorial context) : base(context, LeoState.WaitNearBalcony) { }

        public override void Enter()
        {
            _navigator.MoveTo(_context.BalconyPoint);
        }

        public override void Update()
        {
            base.Update();
            
            if (!_navigator.IsMoving)
            {
                _navigator.SetLookTarget(_context.BalconyPoint); 
            }
        }

        protected override void OnInteract()
        {
            if (_navigator.IsMoving)
                return;

            var dialogue = _context.BalconyDialogue;
            
            dialogue.OnDialogueFinish.AddListener(OnDialogueComplete);
            dialogue.StartDialogue();
        }

        private void OnDialogueComplete()
        {
            _context.BalconyDialogue.OnDialogueFinish.RemoveListener(OnDialogueComplete);
        }
    }
}