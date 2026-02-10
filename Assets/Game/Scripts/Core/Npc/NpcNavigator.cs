using System;
using UnityEngine;
using UnityEngine.AI;
using Game.Scripts.Setups.Core;

namespace Game.Scripts.Core.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcNavigator : MonoBehaviour
    {
        [SerializeField, Space] private NpcSetup _setup;

        private NavMeshAgent _agent;
        private Transform _followTarget;
        private Transform _lookTarget;
        
        private Action _onMovementComplete;
        
        private float _currentSpeed;
        private bool _isStopped = true;
        
        public float MaxSpeed => _setup.MovementSpeed;
        public Vector3 Velocity => _agent.velocity;
        
        public bool IsMoving => !_isStopped; 
        

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            
            _agent.updateRotation = true;
            _agent.updatePosition = true;
            _agent.speed = 0f;
            _agent.acceleration = 999f; 
            _agent.stoppingDistance = _setup.StoppingDistance;
        }

        private void Update()
        {
            if (_followTarget != null && !_isStopped)
            {
                _agent.SetDestination(_followTarget.position);
            }

            HandleMovement();
            HandleRotation();
            
            CheckIfDestinationReached();
        }

        public void MoveTo(Transform target) => MoveTo(target.position, null);

        public void MoveTo(Vector3 targetPosition, Action onComplete = null)
        {
            _followTarget = null;
            _lookTarget = null;
            _isStopped = false;
            
            _onMovementComplete = onComplete;
            
            _agent.isStopped = false;
            _agent.SetDestination(targetPosition);
        }

        public void StartFollowing(Transform target)
        {
            _followTarget = target;
            _lookTarget = target; 
            _isStopped = false;
            _onMovementComplete = null;
            
            _agent.isStopped = false;
        }

        public void Stop()
        {
            _followTarget = null;
            _isStopped = true;
            _onMovementComplete = null;
            
            if (_agent.isOnNavMesh)
            {
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
            }
        }

        public void SetLookTarget(Transform target)
        {
            _lookTarget = target;
        }
        
        public void ClearLookTarget()
        {
            _lookTarget = null;
        }

        private void HandleMovement()
        {
            bool shouldMove = !_isStopped;

            if (shouldMove && !_agent.pathPending)
            {
                if (_agent.remainingDistance <= _setup.StoppingDistance)
                {
                    shouldMove = false;
                }
            }
            
            var targetSpeed = shouldMove ? _setup.MovementSpeed : 0f;
            var isAccelerating = targetSpeed > _currentSpeed;

            var duration = isAccelerating ? _setup.AccelerationTime : _setup.DecelerationTime;
            var curve = isAccelerating ? _setup.AccelerationCurve : _setup.DecelerationCurve;

            var maxSpeed = _setup.MovementSpeed > 0 ? _setup.MovementSpeed : 1f;
            var speedProgress = Mathf.Clamp01(_currentSpeed / maxSpeed);
            
            if (!isAccelerating) speedProgress = 1f - speedProgress;

            var curveValue = curve.Evaluate(speedProgress);
            
            var rate = maxSpeed / Mathf.Max(duration, 0.01f);
            
            var step = rate * (curveValue + 0.1f) * Time.deltaTime;

            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, step);
            _agent.speed = _currentSpeed;

            if (shouldMove || !(_currentSpeed < 0.05f) || _agent.pathPending)
                return;
            
            _isStopped = true;
            _agent.isStopped = true;
        }
        
        private void CheckIfDestinationReached()
        {
            if (_isStopped || !_agent.hasPath || _agent.pathPending) return;
            
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (_onMovementComplete != null)
                {
                    var callback = _onMovementComplete;
                    _onMovementComplete = null;
                    callback.Invoke();
                }
            }
        }

        private void HandleRotation()
        {
            if (_agent.velocity.sqrMagnitude > 0.5f)
                return;

            if (!_lookTarget)
                return;

            var direction = (_lookTarget.position - transform.position).normalized;
            direction.y = 0;

            if (direction == Vector3.zero)
                return;

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _setup.RotationSpeed * Time.deltaTime
            );
        }
    }
}