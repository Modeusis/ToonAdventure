using System;
using Game.Scripts.Core;
using Game.Scripts.Core.Level;
using Game.Scripts.UI.Controls;
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
            
            G.Save.OnLevelChanged.AddListener(HandleLevelChange);
        }

        private void HandleLevelChange(LevelType levelType)
        {
            _continueButton?.SetActiveButton(levelType != 0);
        }
    }
}