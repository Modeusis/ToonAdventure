using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Levels;
using Game.Scripts.UI.Controls;
using Game.Scripts.Utilities.Effects;
using Game.Scripts.Utilities.Events;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.PopUps
{
    [RequireComponent(typeof(CanvasGroupFader))]
    public class FinalPopUp : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private MenuButton _toMainMenuButton;
        [SerializeField] private MenuButton _exitGameButton;
        
        private CanvasGroupFader _fader;
        
        public void Initialize()
        {
            _fader = GetComponent<CanvasGroupFader>();
            
            _fader.IsBlockRaycast = false;
            _fader.CutOut();
            
            _toMainMenuButton.OnClick.AddListener(ToMainMenu);
            _exitGameButton.OnClick.AddListener(ExitGame);
            
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            _fader.IsBlockRaycast = true;
            _fader.FadeIn();
            
            G.EventBus.Publish(new OnPopUpRaycastBlockerEvent { IsBlocking = true });
        }

        private void ToMainMenu()
        {
            G.Save.CurrentLevelId = LevelType.Tutorial;
            G.Scenes.LoadMain().Forget();
            
            Hide();
        }

        private void ExitGame()
        {
            G.Save.CurrentLevelId = LevelType.Tutorial;
            Application.Quit();
            
#if UNITY_EDITOR
            Debug.Log("Quitting game...");
            G.Save.CurrentLevelId = LevelType.Level2;
#endif
            
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _fader.IsBlockRaycast = false;
            _fader.CutOut();
            
            G.EventBus.Publish(new OnPopUpRaycastBlockerEvent { IsBlocking = false });
        }
    }
}