using UnityEngine;

namespace Game.Scripts.Setups.Quests
{
    public abstract class QuestAction : ScriptableObject
    {
        public abstract void Execute();
    }
}