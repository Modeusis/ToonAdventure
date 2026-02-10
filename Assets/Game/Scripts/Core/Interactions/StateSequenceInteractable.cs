using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Interactions
{
    public abstract class StateSequenceInteractable<TState> : InteractableObject 
        where TState : struct, Enum
    {
        [Serializable]
        public struct StateConfig
        {
            [Tooltip("Состояние, для которого настраивается поведение")]
            public TState State;
            
            [Tooltip("Текст взаимодействия для этого состояния (переопределяет базовый InteractionTag)")]
            public string InteractionTagOverride;

            [Tooltip("События, которые произойдут при взаимодействии в этом состоянии")]
            public UnityEvent OnInteract;
        }

        [Header("State Settings")]
        [SerializeField] private TState _initialState;
        [SerializeField] private List<StateConfig> _stateConfigs;

        [Space]
        [Tooltip("Событие, вызываемое при ЛЮБОЙ смене состояния")]
        public UnityEvent<TState> OnStateChanged;

        public TState CurrentState { get; private set; }
        
        private readonly Dictionary<TState, StateConfig> _configMap = new Dictionary<TState, StateConfig>();

        protected virtual void Awake()
        {
            InitializeConfigMap();
        }
        
        protected virtual void Start()
        {
            base.OnInteractionProceed.AddListener(HandleInteraction);
            
            SetState(_initialState, false);
            Hide();
        }
        
        public void SetState(TState newState, bool notify = true, bool showNewTag = true)
        {
            CurrentState = newState;
            
            UpdateInteractionTag(showNewTag);

            if (notify)
            {
                OnStateChanged?.Invoke(CurrentState);
            }
        }
        
        private void HandleInteraction()
        {
            if (_configMap.TryGetValue(CurrentState, out var config))
            {
                config.OnInteract?.Invoke();
            }
        }
        
        private void UpdateInteractionTag(bool showNewTag = true)
        {
            if (!_configMap.TryGetValue(CurrentState, out var config))
                return;
            
            if (string.IsNullOrEmpty(config.InteractionTagOverride))
                return;
            
            InteractionTag = config.InteractionTagOverride;
            
            if (!showNewTag)
                return;
            
            ShowTooltip();
        }

        public void AddListenerToState(TState state, UnityAction action)
        {
            if (_configMap.TryGetValue(state, out var config))
            {
                config.OnInteract.AddListener(action);
            }
        }
        
        public void RemoveListenerFromState(TState state, UnityAction action)
        {
            if (_configMap.TryGetValue(state, out var config))
            {
                config.OnInteract.RemoveListener(action);
            }
        }
        
        public UnityEvent GetEventForState(TState state)
        {
            return _configMap.TryGetValue(state, out var config) ? config.OnInteract : null;
        }
        
        private void InitializeConfigMap()
        {
            foreach (var config in _stateConfigs)
            {
                _configMap.TryAdd(config.State, config);
            }
        }
        
        private void OnDestroy()
        {
            OnInteractionProceed.RemoveListener(HandleInteraction);

            foreach (var config in _configMap)
            {
                config.Value.OnInteract?.RemoveAllListeners();
            }
            
            _configMap.Clear();
        }
    }
}