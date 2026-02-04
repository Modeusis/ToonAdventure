using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New player setup", menuName = "Setup/Player")]
    public class PlayerSetup : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
    }
}