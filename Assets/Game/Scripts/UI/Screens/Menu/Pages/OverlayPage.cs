using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using UnityEngine;

namespace Game.Scripts.UI.Screens.Menu.Pages
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
            _toMainMenuButton.OnClick.AddListener(ShowConfirmToMain);
            
            _leaveButton.OnClick.AddListener(ShowConfirmExit);
        }

        private void CloseMenu()
        {
            G.UI.Screens.Menu.Close();
        }

        private void ShowConfirmToMain()
        {
            G.UI.PopUp.Confirmation.Show("Уровень начнется заново, все равно выйти?", ToMainMenu);
        }
        
        private void ToMainMenu()
        {
            G.Scenes.LoadMain().Forget();
        }
        
        private void ShowConfirmExit()
        {
            G.UI.PopUp.Confirmation.Show("Вы точно хотите выйти?", ExitGame);
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