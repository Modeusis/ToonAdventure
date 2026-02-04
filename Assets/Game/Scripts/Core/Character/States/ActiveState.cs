using UnityEngine;

namespace Game.Scripts.Core.Character.States
{
    public class ActiveState : PlayerBaseState
    {
        private float _currentSpeed;
        private Vector3 _lastMoveDirection;
        
        public ActiveState(Player player) : base(player, PlayerState.Active) { }

        public override void Enter()
        {
            _currentSpeed = 0f;
            _lastMoveDirection = _player.transform.forward;
        }

        public override void Update()
        {
            HandleMovement();
            HandleInput();
        }

        private void HandleMovement()
        {
            var input = G.Input.Game.Move.ReadValue<Vector2>();
            var inputDirection = new Vector3(input.x, 0, input.y);
            
            if (inputDirection.magnitude > 1f) inputDirection.Normalize();

            var targetSpeed = (inputDirection.sqrMagnitude > 0.01f) ? _setup.MovementSpeed : 0;
            var isAccelerating = inputDirection.sqrMagnitude > 0.01f;

            var duration = isAccelerating ? _setup.AccelerationTime : _setup.DecelerationTime;
            var curve = isAccelerating ? _setup.AccelerationCurve : _setup.DecelerationCurve;
            
            var speedProgress = Mathf.Clamp01(_currentSpeed / _setup.MovementSpeed);
            var curveMultiplier = curve.Evaluate(speedProgress);

            var rate = _setup.MovementSpeed / Mathf.Max(duration, 0.001f);
            var step = rate * curveMultiplier * Time.deltaTime;
            
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, step);
            if (inputDirection.sqrMagnitude > 0.01f)
            {
                _lastMoveDirection = inputDirection;
            }
            
            var velocity = _currentSpeed * _lastMoveDirection;
            
            if (!_characterController.isGrounded)
            {
                velocity.y = -_setup.Gravity * Time.deltaTime;
            }
            
            _characterController.Move(velocity * Time.deltaTime);
            
            _animator.SetSpeed(_currentSpeed / _setup.MovementSpeed);

            if (inputDirection == Vector3.zero)
                return;
            
            var targetRotation = Quaternion.LookRotation(inputDirection);
            _player.transform.rotation = Quaternion.RotateTowards(
                _player.transform.rotation, 
                targetRotation, 
                _setup.RotationSpeed * Time.deltaTime
            );
        }

        private void HandleInput()
        {
            if (G.Input.Game.Interact.WasPressedThisFrame())
            {
                // Interact logic
            }

            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                G.UI.Screens.Menu.Toggle();
            }
        }
    }
}