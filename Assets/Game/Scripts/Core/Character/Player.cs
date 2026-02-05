using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Character.States;
using Game.Scripts.Setups.Core;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.StateMachine;
using UnityEngine;

namespace Game.Scripts.Core.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField, Space] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField, Space] public PlayerSetup Setup { get; private set; }

        [SerializeField, Space] private PlayerState _startState;
        
        public CharacterController CharacterController { get; private set; }

        private FSM<PlayerState> _fsm;
        private PlayerState _targetState;

        private bool _isInitialized;

#if UNITY_EDITOR
        public async void Start()
        {
            await UniTask.WaitUntil(() => G.IsReady);
            
            if (!G.IsTestMode)
                return;

            Initialize();
        }
#endif
        
        public void Initialize()
        {
            if (_isInitialized)
                return;
            
            Debug.Log("[Player.Initialize] Call");
            
            CharacterController ??= GetComponent<CharacterController>();
            
            InitializeFSM();
            
            if (!G.IsReady)
                return;
            
            G.EventBus.Subscribe<OnPlayerStateChangeRequest>(OnStateChangeRequested);
            
            _isInitialized = true;
            
            Debug.Log("[Player.Initialize] Initialized");
        }

        private void OnDestroy()
        {
            if (G.IsReady)
                return;
            
            G.EventBus.Unsubscribe<OnPlayerStateChangeRequest>(OnStateChangeRequested);
        }

        private void Update()
        {
            _fsm?.Update();
        }

        private void LateUpdate()
        {
            _fsm?.LateUpdate();
        }

        private void InitializeFSM()
        {
            var states = new Dictionary<PlayerState, State<PlayerState>>
            {
                { PlayerState.Disabled, new DisabledState(this) },
                { PlayerState.Active, new ActiveState(this) },
                { PlayerState.Dialogue, new DialogueState(this) }
            };

            _targetState = _startState;

            var transitions = new List<Transition<PlayerState>>
            {
                new Transition<PlayerState>(PlayerState.Disabled, () => _targetState == PlayerState.Disabled),
                new Transition<PlayerState>(PlayerState.Active, () => _targetState == PlayerState.Active),
                new Transition<PlayerState>(PlayerState.Dialogue, () => _targetState == PlayerState.Dialogue)
            };

            _fsm = new FSM<PlayerState>(states, transitions, _targetState);
        }

        private void OnStateChangeRequested(OnPlayerStateChangeRequest eventData)
        {
            _targetState = eventData.NewState;
        }
    }
}