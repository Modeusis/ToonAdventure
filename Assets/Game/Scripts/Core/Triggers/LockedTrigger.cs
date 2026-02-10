using Game.Scripts.Core.Character;
using Game.Scripts.Core.Loop.Dialogues;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Triggers
{
    public class LockedTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Dialogue _dialogueOnLocked;
        
        [SerializeField, Space] private bool _isLocked = false;
        [SerializeField] private bool _isOneShot = false;
        
        [Header("Actions")]
        [field: SerializeField] public UnityEvent OnUnlock { get; private set; } = new UnityEvent();
        [field: SerializeField] public UnityEvent OnEnter { get; private set; } = new UnityEvent();
        
        private bool _wasInteracted;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_wasInteracted && _isOneShot)
                return;
            
            if (!other.TryGetComponent<Player>(out _))
                return;
                
            if (_isLocked)
            {
                _dialogueOnLocked.StartDialogue();
                return;
            }
            
            OnEnter?.Invoke();

            if (!_isOneShot)
                return;
            
            _wasInteracted = true;
            DisableTrigger();
        }

        public void Unlock()
        {
            _isLocked = false;
            OnUnlock?.Invoke();
        }
        
        private void DisableTrigger()
        {
            var col = GetComponent<Collider>();
            if (col) col.enabled = false;
            
            enabled = false;
        }
    }
}