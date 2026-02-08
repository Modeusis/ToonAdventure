using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Setups.Quests
{
    [CreateAssetMenu(fileName = "New QuestLine", menuName = "Setup/Quest/Database")]
    public class QuestLineData : ScriptableObject
    {
        [SerializeField] private List<QuestData> _quests;

        public QuestData GetQuestByIndex(int index)
        {
            if (index >= 0 && index < _quests.Count)
                return _quests[index];
            
            return null;
        }
        
        public int GetTotalQuests() => _quests.Count;
    }
}