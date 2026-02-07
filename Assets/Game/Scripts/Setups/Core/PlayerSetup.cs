using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New player setup", menuName = "Setup/Core/Player")]
    public class PlayerSetup : ScriptableObject
    {
        [field: Header("Movement Settings")]
        [field: SerializeField, Space] public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 720f;
        [field: SerializeField] public float Gravity { get; private set; } = 9.8f;
        
        [field: Header("Acceleration Dynamics")]
        [field: SerializeField, Space] public float AccelerationTime { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve AccelerationCurve { get; private set; } = AnimationCurve.Linear(0, 1, 1, 1);
        
        [field: Header("Deceleration Dynamics")]
        [field: SerializeField, Space] public float DecelerationTime { get; private set; } = 0.5f;
        [field: SerializeField] public AnimationCurve DecelerationCurve { get; private set; } = AnimationCurve.Linear(0, 1, 1, 1);
    }
}