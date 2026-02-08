using System.Linq;
using Game.Scripts.Setups.Quests;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Screens.Quest
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField, Space] private TMP_Text _questTitleText;
        [SerializeField] private TMP_Text _currentObjectiveText;

        public void Show(QuestData quest)
        {
            if (!quest)
            {
                Hide();
                return;
            }

            gameObject.SetActive(true);
            Refresh(quest);
        }

        public void Refresh(QuestData quest)
        {
            _questTitleText.text = quest.Title;
            
            var currentStep = quest.Steps.FirstOrDefault(s => !s.IsCompleted);

            if (currentStep != null)
            {
                _currentObjectiveText.text = $"- {currentStep.Description}";
            }
            else
            {
                _currentObjectiveText.text = "- Задание выполнено";
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}