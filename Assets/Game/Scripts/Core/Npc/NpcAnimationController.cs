using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Core.NPC
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcAnimationController : MonoBehaviour
    {
        [Header("Parameters Name")]
        [SerializeField] private string _speedParameterName = "Speed";
        [SerializeField] private string _actionParameterName = "Happy";
        
        [SerializeField, Space] private float _animationSmoothTime = 0.1f;

        private int _speedKey;
        private int _actionKey;
        
        private NavMeshAgent _agent;
        private Animator _animator;
        private NpcNavigator _navigator;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navigator = GetComponent<NpcNavigator>();

            _speedKey = Animator.StringToHash(_speedParameterName);
            _actionKey = Animator.StringToHash(_actionParameterName);
        }

        private void Update()
        {
            SynchronizeSpeed();
        }

        public void PlayAction()
        {
            _animator.SetTrigger(_actionKey);
        }
        
        private void SynchronizeSpeed()
        {
            var currentVelocity = _agent.velocity.magnitude;
            var maxSpeed = _navigator != null ? _navigator.MaxSpeed : _agent.speed;
            
            if (maxSpeed <= 0) maxSpeed = 1f;

            var normalizedSpeed = Mathf.Clamp01(currentVelocity / maxSpeed);

            _animator.SetFloat(_speedKey, normalizedSpeed, _animationSmoothTime, Time.deltaTime);
        }
    }
}