using Game.Scripts.Core.Levels;
using UnityEngine;

namespace Game.Scripts.Setups.Levels
{
    [CreateAssetMenu(fileName = "New levels setup", menuName = "Setup/Levels")]
    public class LevelSetup : EnumDictionarySetup<LevelType, Level>
    {
        
    }
}