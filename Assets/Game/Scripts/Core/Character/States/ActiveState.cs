using UnityEngine;

namespace Game.Scripts.Core.Character.States
{
    public class ActiveState : PlayerBaseState
    {
        public ActiveState(Player player) : base(player, PlayerState.Active) { }

        public override void Update()
        {
            HandleMovement();
            HandleInput();
        }

        private void HandleMovement()
        {
            Vector2 input = G.Input.Game.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            
            if (move.magnitude > 1f) move.Normalize();

            _characterController.Move(move * _setup.MovementSpeed * Time.deltaTime);

            _animator.SetSpeed(move.magnitude);

            if (move != Vector3.zero)
            {
                _player.transform.forward = move;
            }
        }

        private void HandleInput()
        {
            if (G.Input.Game.Interact.WasPressedThisFrame())
            {
                // Interact logic
            }

            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                // Open Menu logic
            }
        }
    }
}