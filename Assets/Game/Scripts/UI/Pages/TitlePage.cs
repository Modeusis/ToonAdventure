using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core;
using Game.Scripts.Core.Levels;
using Game.Scripts.UI.Controls;
using Game.Scripts.Utilities.Constants;
using UnityEngine;

namespace Game.Scripts.UI.Pages
{
    public class TitlePage : Page
    {
        [SerializeField, Space] private MenuButton _continueButton;
        [SerializeField] private MenuButton _newGameButton;
        [SerializeField] private MenuButton _settingsButton;
        [SerializeField] private MenuButton _leaveButton;

        public override void Initialize()
        {
            _settingsButton.OnClick.AddListener(() => ToPage(PageType.Settings));
            
            _continueButton.OnClick.AddListener(LoadGameplay);
            _newGameButton.OnClick.AddListener(LoadNewGame);
            
            _leaveButton.OnClick.AddListener(ExitGame);
            
            G.Save.OnLevelChanged.AddListener(HandleLevelChange);
        }

        private void HandleLevelChange(LevelType levelType)
        {
            _continueButton?.SetActiveButton(levelType != 0);
        }

        private void LoadNewGame()
        {
            G.Save.CurrentLevelId = LevelType.Tutorial;

            LoadGameplay();
        }

        private void LoadGameplay()
        {
            G.Scenes.LoadGameplay().Forget();
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