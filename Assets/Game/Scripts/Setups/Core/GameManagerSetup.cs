using UnityEngine;

namespace Game.Scripts.Setups.Core
{
    [CreateAssetMenu(fileName = "New game manager setup", menuName = "Setup/G")]
    public class GameManagerSetup : ScriptableObject
    {
        [Header("Scene loading parameters")]
        [field: SerializeField, Space] public float LoadDelay { get; private set; }
    }
}