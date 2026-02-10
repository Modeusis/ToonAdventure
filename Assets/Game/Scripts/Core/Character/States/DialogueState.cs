using Game.Scripts.Utilities.Events;

namespace Game.Scripts.Core.Character.States
{
    public class DialogueState : PlayerBaseState
    {
        public DialogueState(Player player) : base(player, PlayerState.Dialogue) { }

        public override void Enter()
        {
            _animator.SetSpeed(0);
            G.Cursor.Unlock();
        }

        public override void Update()
        {
            if (G.Input.Game.Interact.WasPressedThisFrame())
            {
                G.EventBus.Publish(new ContinueDialogueRequestEvent());
            }

            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                G.UI.Screens.Menu.Toggle();
            }
        }
    }
}