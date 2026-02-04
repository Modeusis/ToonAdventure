using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using Game.Scripts.Utilities.Constants;
using UnityEngine;

namespace Game.Scripts.UI.Pages
{
    public class OverlayPage : Page
    {
        [SerializeField, Space] private MenuButton _continueButton;
        [SerializeField] private MenuButton _settingsButton;
        [SerializeField] private MenuButton _toMainMenuButton;
        [SerializeField] private MenuButton _leaveButton;
        
        public override void Initialize()
        {
            _continueButton.OnClick.AddListener(CloseMenu);
            
            _settingsButton.OnClick.AddListener(() => ToPage(PageType.Settings));
            _toMainMenuButton.OnClick.AddListener(ToMainMenu);
            
            _leaveButton.OnClick.AddListener(ExitGame);
        }

        private void CloseMenu()
        {
            G.UI.Screens.Menu.Close();
        }

        private void ToMainMenu()
        {
            G.Scenes.LoadMain().Forget();
        }
        
        private void ExitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            Debug.Log("Quitting game...");
#endif
        }
    }
}