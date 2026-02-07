using System;
using Game.Scripts.Core.Levels;
using UnityEngine;

namespace Game.Scripts.Setups.Levels
{
    [CreateAssetMenu(fileName = "New levels setup", menuName = "Setup/Levels")]
    public class LevelSetup : EnumDictionarySetup<LevelType, LevelInfo>
    {
        [field: SerializeField] public float LevelLoadDelay { get; private set; } = 1.5f;
    }

    [Serializable]
    public class LevelInfo
    {
        [field: SerializeField] public string LoadTag { get; private set; }
        [field: SerializeField] public Level Prefab { get; private set; }
    }
}