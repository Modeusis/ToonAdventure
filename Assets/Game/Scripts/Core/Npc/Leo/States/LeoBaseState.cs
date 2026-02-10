using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.NPC;
using Game.Scripts.Core.NPC.Leo;
using Game.Scripts.Utilities.StateMachine;

namespace Game.Scripts.Core.Npc.Leo.States
{
    public abstract class LeoBaseState : State<LeoState>
    {
        protected readonly LeoTutorial _context;
        
        protected readonly NpcNavigator _navigator;
        protected readonly NpcAnimationController _animator;
        protected readonly InteractableObject _interactable;

        protected LeoBaseState(LeoTutorial context, LeoState stateType)
        {
            _context = context;
            
            _navigator = context.Navigator;
            _interactable = context.Interactable;
            _animator = context.Animator;
                
            StateType = stateType;
        }

        public override void Enter() { }
        public override void Exit() { }

        public override void Update() { }

        protected abstract void OnInteract();
    }
}