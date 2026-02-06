using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Character.States;
using Game.Scripts.Core.Interactions;
using Game.Scripts.Setups.Core;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.StateMachine;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Core.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField, Space] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField] public CinemachineInputAxisController CameraInputController { get; private set; }
        [field: SerializeField, Space] public PlayerSetup Setup { get; private set; }
        
        [SerializeField, Space] private PlayerState _startState;
        
        public CharacterController CharacterController { get; private set; }

        private FSM<PlayerState> _fsm;
        private IInteractable _currentInteractable;
        
        private PlayerState _stateBeforePause;
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
            
            CharacterController ??= GetComponent<CharacterController>();
            
            InitializeFSM();
            
            if (!G.IsReady)
                return;
            
            G.EventBus.Subscribe<OnDialogueStateChangedEvent>(OnDialogueStateChanged);
            G.EventBus.Subscribe<OnGamePausedEvent>(OnGamePaused);
            G.EventBus.Subscribe<OnPlayerStateChangeRequest>(OnPauseStateChangeRequested);
            G.EventBus.Subscribe<OnInteractionZoneEnterEvent>(OnInteractionZoneEnter);
            G.EventBus.Subscribe<OnInteractionZoneExitEvent>(OnInteractionZoneExit);
            
            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (G.IsReady)
                return;
            
            G.EventBus.Unsubscribe<OnDialogueStateChangedEvent>(OnDialogueStateChanged);
            G.EventBus.Unsubscribe<OnGamePausedEvent>(OnGamePaused);
            G.EventBus.Unsubscribe<OnPlayerStateChangeRequest>(OnPauseStateChangeRequested);
            G.EventBus.Unsubscribe<OnInteractionZoneEnterEvent>(OnInteractionZoneEnter);
            G.EventBus.Unsubscribe<OnInteractionZoneExitEvent>(OnInteractionZoneExit);
        }

        private void Update()
        {
            _fsm?.Update();
        }

        private void LateUpdate()
        {
            _fsm?.LateUpdate();
        }

        public void TryInteract()
        {
            if (_currentInteractable == null)
                return;
            
            _currentInteractable.Interact();
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

        private void OnInteractionZoneEnter(OnInteractionZoneEnterEvent eventData)
        {
            _currentInteractable = eventData.Interactable;
        }

        private void OnInteractionZoneExit(OnInteractionZoneExitEvent eventData)
        {
            if (_currentInteractable != eventData.Interactable)
                return;
            
            _currentInteractable = null;
        }

        private void OnDialogueStateChanged(OnDialogueStateChangedEvent eventData)
        {
            if (eventData.IsActive)
            {
                G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Dialogue });
            }
            else
            {
                G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Active });
            }
        }
        
        private void OnPauseStateChangeRequested(OnPlayerStateChangeRequest eventData)
        {
            _targetState = eventData.NewState;
        }
        
        private void OnGamePaused(OnGamePausedEvent eventData)
        {
            if (eventData.IsPaused)
            {
                _stateBeforePause = _targetState; 
                
                G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Disabled });
            }
            else
            {
                G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = _stateBeforePause });
            }
        }
    }
}