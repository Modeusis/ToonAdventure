namespace Game.Scripts.Core.Character.States
{
    public class DisabledState : PlayerBaseState
    {
        public DisabledState(Player player) : base(player, PlayerState.Disabled) { }

        public override void Enter()
        {
            _animator.SetSpeed(0);
        }

        public override void Update()
        {
            if (G.Input.Game.Back.WasPerformedThisFrame())
            {
                G.UI.Screens.Menu.Toggle();
            }
        }
    }
}