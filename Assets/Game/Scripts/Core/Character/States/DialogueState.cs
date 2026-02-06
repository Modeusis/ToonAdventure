namespace Game.Scripts.Core.Character.States
{
    public class DialogueState : PlayerBaseState
    {
        public DialogueState(Player player) : base(player, PlayerState.Dialogue) { }

        public override void Enter()
        {
            _animator.SetSpeed(0);
        }

        public override void Update()
        {
            if (G.Input.Game.Interact.WasPressedThisFrame())
            {
                
            }

            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                
            }
        }
    }
}