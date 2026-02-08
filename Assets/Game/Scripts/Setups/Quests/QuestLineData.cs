using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Setups.Quests
{
    [CreateAssetMenu(fileName = "New QuestLine", menuName = "Setup/Quest/Database")]
    public class QuestLineData : ScriptableObject
    {
        [SerializeField] private List<QuestData> _quests;

        public QuestData GetQuestById(string questId)
        {
            return _quests.FirstOrDefault(quest => quest.Id == questId);
        }
    }
}