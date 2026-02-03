using System;
using Game.Scripts.Core.Level;

namespace Game.Scripts.Data
{
    [Serializable]
    public class GameProgressData
    {
        public LevelType CurrentLevelId = LevelType.Tutorial;
    }
}