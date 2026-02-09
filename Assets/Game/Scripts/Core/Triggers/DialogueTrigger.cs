using Game.Scripts.Core.Character;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Triggers
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField, Space] private TextAsset _inkFile;
        [SerializeField] private string _startPath = "default_knot";
        
        [SerializeField, Space] private bool _isOneShot;

        [field: SerializeField] public UnityEvent OnDialogueFinish { get; private set; } = new UnityEvent();
        
        private bool _wasInteracted;

        private void OnTriggerEnter(Collider other)
        {
            if (_wasInteracted && _isOneShot)
                return;

            if (!other.TryGetComponent<Player>(out _))
                return;
                
            G.EventBus.Publish(new StartDialogueRequestEvent
            {
                InkAsset = _inkFile,
                StartPath = _startPath,
                OnFinished = OnDialogueFinish
            });

            if (!_isOneShot)
                return;
            
            _wasInteracted = true;
            DisableTrigger();
        }

        private void DisableTrigger()
        {
            var col = GetComponent<Collider>();
            if (col) col.enabled = false;
            
            enabled = false;
        }
    }
}