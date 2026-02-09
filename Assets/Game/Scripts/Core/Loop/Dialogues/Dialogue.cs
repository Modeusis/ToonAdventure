using System;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Loop.Dialogues
{
    [Serializable]
    public class Dialogue
    {
        [Header("Ink Settings")]
        [SerializeField] private TextAsset _inkFile;
        [SerializeField] private string _startPath = "template_knot";
        
        [Header("Behaviour")]
        [SerializeField] private bool _isOneShot;

        [Header("Events")]
        [field: SerializeField] public UnityEvent OnDialogueFinish { get; private set; } = new UnityEvent();
        
        private bool _hasPlayed;

        public void StartDialogue()
        {
            if (_isOneShot && _hasPlayed)
            {
                return;
            }
            
            G.EventBus.Publish(new StartDialogueRequestEvent
            {
                InkAsset = _inkFile,
                StartPath = _startPath,
                OnFinished = OnDialogueFinish
            });
            
            if (_isOneShot)
            {
                _hasPlayed = true;
            }
        }

        private void OnDestroy()
        {
            OnDialogueFinish?.RemoveAllListeners();
        }
    }
}