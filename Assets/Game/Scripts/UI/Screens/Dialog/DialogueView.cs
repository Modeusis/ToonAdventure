using System;
using System.Collections.Generic;
using Game.Scripts.UI.Controls;
using Game.Scripts.Utilities.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Screens.Dialog
{
    public class DialogueView : MonoBehaviour
    {
        [Header("General")]
        private CanvasGroupFader _dialogueFader;
        
        [Header("Speech")]
        [SerializeField] private TMP_Text _speakerNameText;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private Image _portraitImage;
        
        [Header("Choices")]
        [SerializeField] private Transform _choicesContainer;
        [SerializeField] private MenuButton _choiceButtonPrefab;

        private List<MenuButton> _activeChoices = new List<MenuButton>();

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public void SetContent(string speakerName, string text, Sprite portrait)
        {
            _speakerNameText.text = speakerName;
            _dialogueText.text = text;
            
            //TODO add effect for text appearance with cancellation token
            
            if (portrait != null)
            {
                _portraitImage.sprite = portrait;
                _portraitImage.gameObject.SetActive(true);
            }
            else
            {
                _portraitImage.gameObject.SetActive(false);
            }
        }

        public void ClearChoices()
        {
            foreach (var button in _activeChoices)
            {
                Destroy(button.gameObject);
            }
            _activeChoices.Clear();
        }

        public void CreateChoice(string text, Action onClick)
        {
            var button = Instantiate(_choiceButtonPrefab, _choicesContainer);
            var tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) tmp.text = text;
            
            button.OnClick.AddListener(() => onClick?.Invoke());
            _activeChoices.Add(button);
        }
    }
}