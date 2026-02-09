using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New NPC setup", menuName = "Setup/Core/NPC")]
    public class NpcSetup : ScriptableObject
    {
        [field: Header("Movement Settings")]
        [field: SerializeField, Space] public float MovementSpeed { get; private set; } = 3.5f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 360f;
        
        [field: Header("Acceleration Dynamics")]
        [field: SerializeField, Space] public float AccelerationTime { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve AccelerationCurve { get; private set; } = AnimationCurve.Linear(0, 0, 1, 1);
        
        [field: Header("Deceleration Dynamics")]
        [field: SerializeField, Space] public float DecelerationTime { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve DecelerationCurve { get; private set; } = AnimationCurve.Linear(0, 0, 1, 1);
        
        [field: Header("Navigation")]
        [field: SerializeField, Space] public float StoppingDistance { get; private set; } = 1.5f;
    }
}