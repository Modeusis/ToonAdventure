using System.Collections.Generic;
using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.Core.Npc.Leo.States;
using Game.Scripts.Data;
using Game.Scripts.Utilities.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Core.NPC.Leo
{
    [RequireComponent(typeof(NpcNavigator))]
    public class LeoTutorial : MonoBehaviour
    {
        [SerializeField, Space] private NpcNavigator _navigator;
        [SerializeField] private NpcAnimationController _animator;
        [SerializeField] private InteractableObject _interactable;

        [Header("Dialogues")]
        [SerializeField] private Dialogue _greetingDialogue;
        [SerializeField] private Dialogue _balconyDialogue;
        [SerializeField] private Dialogue _toyFoundDialogue;
        
        [Header("Debug")]
        [SerializeField] private LeoState _startState;
        [SerializeField] private LeoState _currentStateView;

        private Transform _balconyPoint;
        private Transform _toyPoint;
        
        private FSM<LeoState> _fsm;
        private LeoState _targetState;
        
        public NpcNavigator Navigator => _navigator;
        public NpcAnimationController Animator => _animator;
        public InteractableObject Interactable => _interactable;
        public Transform BalconyPoint => _balconyPoint;
        public Transform ToyPoint => _toyPoint;
        
        public Dialogue GreetingDialogue => _greetingDialogue;
        public Dialogue BalconyDialogue => _balconyDialogue;
        public Dialogue ToyFoundDialogue => _toyFoundDialogue;

        public void Initialize(LeoTutorialData data)
        {
            if (data == null)
            {
                Debug.LogError($"Tutorial data can't be null!");
                return;
            }

            _balconyPoint = data.BalconyTransform;
            _toyPoint = data.ToyFoundTransform;
            
            _navigator ??= GetComponent<NpcNavigator>();
            InitializeFSM();
        }

        private void Update()
        {
            _fsm?.Update();
            _currentStateView = _targetState;
        }

        private void LateUpdate()
        {
            _fsm?.LateUpdate();
        }

        public void SetState(LeoState newState)
        {
            _targetState = newState;
        }

        private void InitializeFSM()
        {
            var states = new Dictionary<LeoState, State<LeoState>>
            {
                { LeoState.FirstInteraction, new LeoFirstInteractionState(this) },
                { LeoState.WaitNearBalcony, new LeoWaitNearBalconyState(this) },
                { LeoState.ToyFound, new LeoToyFoundState(this) },
                { LeoState.Finished, new LeoFinishedState(this) }
            };

            _targetState = _startState;

            var transitions = new List<Transition<LeoState>>
            {
                new Transition<LeoState>(LeoState.FirstInteraction, () => _targetState == LeoState.FirstInteraction),
                new Transition<LeoState>(LeoState.WaitNearBalcony, () => _targetState == LeoState.WaitNearBalcony),
                new Transition<LeoState>(LeoState.ToyFound, () => _targetState == LeoState.ToyFound),
                new Transition<LeoState>(LeoState.Finished, () => _targetState == LeoState.Finished)
            };

            _fsm = new FSM<LeoState>(states, transitions, _targetState);
        }
    }
}