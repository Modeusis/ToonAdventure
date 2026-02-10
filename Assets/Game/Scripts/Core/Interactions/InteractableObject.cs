using Game.Scripts.Core.Character;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Scripts.Core.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField, Space] private bool _isOneShot;
        
        [field: SerializeField, Space] public string InteractionTag { get; set; } = "взаимодействовать";
        [field: SerializeField] public InteractionAnimationType Type { get; private set; } = InteractionAnimationType.None;
        
        [field: SerializeField, Space] public UnityEvent<Transform> OnInteractionZoneEnter { get; private set; } = new UnityEvent<Transform>();
        [field: SerializeField] public UnityEvent OnInteractionZoneExit { get; private set; } = new UnityEvent();
        [field: SerializeField] public UnityEvent OnInteractionProceed { get; private set; } = new UnityEvent();
        
        private Collider _collider;
        
        private bool _wasInteracted;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_wasInteracted && _isOneShot)
                return;

            if (!other.TryGetComponent<Player>(out _))
                return;

            ShowTooltip();
            
            OnInteractionZoneEnter?.Invoke(other.transform);
                
            G.EventBus.Publish(new OnInteractionZoneEnterEvent
            {
                Interactable = this,
                Transform = transform
            });
        }

        private void OnTriggerExit(Collider other)
        {
            if (_wasInteracted && _isOneShot) return;

            if (!other.TryGetComponent<Player>(out _))
                return;
            
            Hide();
            
            OnInteractionZoneExit?.Invoke();
            
            G.EventBus.Publish(new OnInteractionZoneExitEvent
            {
                Interactable = this
            });
        }

        public void Interact()
        {
            if (_wasInteracted && _isOneShot) return;
            
            OnInteractionProceed?.Invoke();
            
            if (_isOneShot)
            {
                _wasInteracted = true;
                DisableInteraction();
            }
        }

        private void DisableInteraction()
        {
            Hide();
            
            OnInteractionZoneExit?.Invoke();
            
            G.EventBus.Publish(new OnInteractionZoneExitEvent
            {
                Interactable = this
            });
            
            if (_collider) _collider.enabled = false;
            
            enabled = false;
        }

        protected void ShowTooltip()
        {
            var inputBinding = G.Input.Game.Interact.GetBindingDisplayString();
            G.UI.Screens.HUD.DynamicTooltip.Show($"{inputBinding} - {InteractionTag}");
        }

        protected void Hide()
        {
            G.UI.Screens.HUD.DynamicTooltip.Hide();
        }
    }
}