using System;
using Game.Scripts.Core.Cameras.Cursors;
using Game.Scripts.Core.Character.States;
using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New game manager setup", menuName = "Setup/Core/Game")]
    public class GameManagerSetup : ScriptableObject
    {
        [Header("Scene loading parameters")]
        [field: SerializeField, Space] public float LoadDelay { get; private set; }
        
        [Header("Camera")]
        [field: SerializeField] public CameraManagerSettings CameraSettings { get; private set; }
        
        [Header("Cursor")]
        [field: SerializeField] public CursorState StartCursorState { get; private set; } = CursorState.Active;
    }

    [Serializable]
    public class CameraManagerSettings
    {
        [field: SerializeField, Space] public int InactivePriority { get; private set; } 
        [field: SerializeField] public int ActivePriority { get; private set; }
    }
}