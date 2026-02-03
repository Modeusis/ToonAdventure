using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using UnityEngine;

namespace Game.Scripts.UI.Pages
{
    public class SettingsPage : Page
    {
        [Header("Sliders")]
        [SerializeField, Space] private MenuSlider _musicSlider;
        [SerializeField] private MenuSlider _sfxSlider;
        
        [Header("Buttons")]
        [SerializeField] private MenuButton _toMainButton;
        [SerializeField] private MenuButton _saveButton;
        
        public override void Initialize()
        {
            _toMainButton.OnClick.AddListener(() =>
            {
                ToPage(PageType.Main);
            });
            
            _saveButton.OnClick.AddListener(() =>
            {
                //TODO Add save logic
            });
        }

        public override void Show()
        {
            //TODO Add initial load
            
            _saveButton.SetActiveButton(false);
            
            base.Show();
        }
    }
}