using System;
using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using Game.Scripts.Utilities.Events;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.PopUps
{
    public class ConfirmationPopUp : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private MenuButton _confirmButton;
        [SerializeField] private MenuButton _closeButton;
        
        [Header("Button Labels")]
        [SerializeField] private TMP_Text _confirmLabel;
        [SerializeField] private TMP_Text _closeLabel;
        
        private Action _onConfirmAction;
        private Action _onCloseAction;

        public void Initialize()
        {
            _confirmButton.OnClick.AddListener(OnConfirmClicked);
            _closeButton.OnClick.AddListener(OnCloseClicked);
            
            gameObject.SetActive(false);
        }
        
        public void Show(
            string message, 
            Action onConfirm, 
            Action onClose = null,
            string confirmText = "Confirm", 
            string closeText = "Cancel")
        {
            _messageText.text = message;
            
            _onConfirmAction = onConfirm;
            _onCloseAction = onClose;
            
            if (_confirmLabel) _confirmLabel.text = confirmText;
            if (_closeLabel) _closeLabel.text = closeText;

            gameObject.SetActive(true);
            
            G.EventBus.Publish(new OnPopUpRaycastBlockerEvent { IsBlocking = true });
        }

        private void OnConfirmClicked()
        {
            _onConfirmAction?.Invoke();
            Hide();
        }

        private void OnCloseClicked()
        {
            _onCloseAction?.Invoke();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _onConfirmAction = null;
            _onCloseAction = null;
            
            G.EventBus.Publish(new OnPopUpRaycastBlockerEvent { IsBlocking = false });
        }
    }
}