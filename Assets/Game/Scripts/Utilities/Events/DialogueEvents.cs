using System;
using UnityEngine;
using UnityEngine.Events;

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
        public UnityEvent OnFinished;
    }

    public struct OnTagResievedEvent
    {
        public string Key;
        public string Value;
    }
    
    public struct ContinueDialogueRequestEvent { }
}