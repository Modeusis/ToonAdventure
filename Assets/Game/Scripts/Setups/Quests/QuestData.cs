using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Setups.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Setup/Quest/Data")]
    public class QuestData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        
        [SerializeField] private List<QuestStep> _steps;
        [SerializeField] private List<QuestAction> _actionsOnComplete;
        
        public IReadOnlyList<QuestStep> Steps => _steps;
        public IReadOnlyList<QuestAction> ActionsOnComplete => _actionsOnComplete;
        
        public void ResetProgress()
        {
            foreach (var step in _steps)
            {
                step.CurrentAmount = 0;
                step.IsCompleted = false;
            }
        }
    }
    
    [Serializable]
    public class QuestStep
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public string TargetId { get; private set; }
        [field: SerializeField] public int RequiredAmount { get; private set; } = 1;
        [field: SerializeField] public bool IsCompleted { get; set; }
        [field: SerializeField] public int CurrentAmount { get; set; }
    }
}