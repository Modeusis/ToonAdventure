using UnityEngine;

namespace Game.Scripts.Core.Character.States
{
    public class ActiveState : PlayerBaseState
    {
        private float _currentSpeed;
        private float _verticalVelocity;
        
        private Vector3 _lastMoveDirection;
        
        public ActiveState(Player player) : base(player, PlayerState.Active) { }

        public override void Enter()
        {
            ResetKinematics();
            
            _cameraController.enabled = true;
        }

        public override void Update()
        {
            CalculateHorizontalMovement(out var hVelocity);
            HandleGravity();
            
            var finalVelocity = hVelocity;
            finalVelocity.y = _verticalVelocity;
            
            _characterController.Move(finalVelocity * Time.deltaTime);
            
            HandleInput();
        }

        public override void Exit()
        {
            _cameraController.enabled = false;
        }

        private void CalculateHorizontalMovement(out Vector3 horizontalVelocity)
        {
            var input = G.Input.Game.Move.ReadValue<Vector2>();

            MapInputToCamera(input,out var inputDirection);
            
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
                
                var targetRotation = Quaternion.LookRotation(inputDirection);
                _player.transform.rotation = Quaternion.RotateTowards(
                    _player.transform.rotation, 
                    targetRotation, 
                    _setup.RotationSpeed * Time.deltaTime
                );
            }
            
            _animator.SetSpeed(_currentSpeed / _setup.MovementSpeed);
            
            horizontalVelocity = _currentSpeed * _lastMoveDirection;
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded)
            {
                if (_verticalVelocity < 0)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                _verticalVelocity -= _setup.Gravity * Time.deltaTime;
            }
        }
        
        private void HandleInput()
        {
            if (G.Input.Game.Interact.WasPressedThisFrame())
            {
                _player.TryInteract();
            }

            if (G.Input.Game.Back.WasPressedThisFrame())
            {
                G.UI.Screens.Menu.Toggle();
            }
        }

        private void MapInputToCamera(Vector3 input, out Vector3 mappedInput)
        {

            var cameraTransform = _cameraController.transform;
            var cameraForward = cameraTransform.forward;
            var cameraRight = cameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            mappedInput = cameraForward * input.y + cameraRight * input.x;
        }
        
        private void ResetKinematics()
        {
            _currentSpeed = 0f;
            _verticalVelocity = 0f;
            
            _lastMoveDirection = _player.transform.forward;
        }
    }
}