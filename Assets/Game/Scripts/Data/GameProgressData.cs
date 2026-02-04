using System;
using Game.Scripts.Core.Levels;

namespace Game.Scripts.Data
{
    [Serializable]
    public class GameProgressData
    {
        public LevelType CurrentLevelId = LevelType.Tutorial;
    }
}