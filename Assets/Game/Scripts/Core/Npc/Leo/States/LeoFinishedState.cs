using Game.Scripts.Core.NPC.Leo;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public class LeoFinishedState : LeoBaseState
    {
        public LeoFinishedState(LeoTutorial context) : base(context, LeoState.Finished) { }

        public override void Enter()
        {
            _navigator.Stop();
        }

        protected override void OnInteract()
        {
            
        }
    }
}