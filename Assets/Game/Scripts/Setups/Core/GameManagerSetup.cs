using System;
using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New game manager setup", menuName = "Setup/G")]
    public class GameManagerSetup : ScriptableObject
    {
        [Header("Scene loading parameters")]
        [field: SerializeField, Space] public float LoadDelay { get; private set; }
        
        [Header("Managers")]
        [field: SerializeField] public CameraManagerSettings CameraSettings { get; private set; }
    }

    [Serializable]
    public class CameraManagerSettings
    {
        [field: SerializeField, Space] public int InactivePriority { get; private set; } 
        [field: SerializeField] public int ActivePriority { get; private set; }
    }
}