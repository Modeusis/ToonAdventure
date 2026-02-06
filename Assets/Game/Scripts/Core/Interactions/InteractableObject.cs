using Game.Scripts.Core.Character;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField, Space] private bool _isOneShot;

        [field: SerializeField, Space] public UnityEvent OnInteractionZoneEnter { get; private set; } = new UnityEvent();
        [field: SerializeField] public UnityEvent OnInteractionZoneExit { get; private set; } = new UnityEvent();
        [field: SerializeField] public UnityEvent OnInteractionProceed { get; private set; } = new UnityEvent();

        private bool _wasInteracted;

        private void OnTriggerEnter(Collider other)
        {
            if (_wasInteracted && _isOneShot) return;

            if (!other.TryGetComponent<Player>(out _))
                return;
            
            OnInteractionZoneEnter?.Invoke();
                
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
            OnInteractionZoneExit?.Invoke();
            
            G.EventBus.Publish(new OnInteractionZoneExitEvent
            {
                Interactable = this
            });

            var col = GetComponent<Collider>();
            if (col) col.enabled = false;
            
            enabled = false;
        }
    }
}