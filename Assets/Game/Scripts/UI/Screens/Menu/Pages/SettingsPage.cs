using Game.Scripts.Core;
using Game.Scripts.UI.Controls;
using UnityEngine;

namespace Game.Scripts.UI.Screens.Menu.Pages
{
    public class SettingsPage : Page
    {
        [Header("Sliders")]
        [SerializeField, Space] private MenuSlider _musicSlider;
        [SerializeField] private MenuSlider _sfxSlider;
        
        [Header("Buttons")]
        [SerializeField] private MenuButton _backButton;
        [SerializeField] private MenuButton _saveButton;

        private float _savedSfxVolume;
        private float _savedMusicVolume;
        
        public override void Initialize()
        {
            OnClosed.AddListener(SetDefaults);
            
            _backButton.OnClick.AddListener(() =>
            {
                G.UI.Screens.Menu.ToPreviousPage();
            });
            
            _saveButton.OnClick.AddListener(() =>
            {
                G.Save.SfxVolume = _sfxSlider.GetValue();
                G.Save.MusicVolume = _musicSlider.GetValue();
                
                UpdateBaselines();
                CheckChanges();
            });

            _musicSlider.OnValueChanged.AddListener(HandleMusicSliderChanged);
            _sfxSlider.OnValueChanged.AddListener(HandleSfxSliderChanged);
        }

        public override void Show()
        {
            UpdateBaselines();
            
            _sfxSlider.SetValue(_savedSfxVolume, notifyListeners: false);
            _musicSlider.SetValue(_savedMusicVolume, notifyListeners: false);
            
            _saveButton.SetActiveButton(false);
            
            base.Show();
        }
        
        private void UpdateBaselines()
        {
            _savedSfxVolume = G.Save.SfxVolume;
            _savedMusicVolume = G.Save.MusicVolume;
        }

        private void SetDefaults()
        {
            G.Audio.SetMusicVolume(_savedMusicVolume);
            G.Audio.SetSfxVolume(_savedSfxVolume);
        }
        
        private void HandleMusicSliderChanged(float value)
        {
            G.Audio.SetMusicVolume(value);
            
            CheckChanges();
        }

        private void HandleSfxSliderChanged(float value)
        {
            G.Audio.SetSfxVolume(value);
            
            CheckChanges();
        }
        
        private void CheckChanges()
        {
            var currentMusic = _musicSlider.GetValue();
            var currentSfx = _sfxSlider.GetValue();

            var isMusicChanged = !Mathf.Approximately(_savedMusicVolume, currentMusic);
            var isSfxChanged = !Mathf.Approximately(_savedSfxVolume, currentSfx); 

            _saveButton.SetActiveButton(isMusicChanged || isSfxChanged);
        }
    }
}