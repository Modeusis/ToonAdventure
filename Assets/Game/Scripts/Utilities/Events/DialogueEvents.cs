using UnityEngine;

namespace Game.Scripts.Utilities.Events
{

    public struct OnDialogueStateChangedEvent
    {
        public bool IsActive;
    }
    
    public struct StartDialogueRequestEvent
    {
        public TextAsset InkAsset;
        public string StartPath;
    }
    
    public struct ContinueDialogueRequestEvent { }
}