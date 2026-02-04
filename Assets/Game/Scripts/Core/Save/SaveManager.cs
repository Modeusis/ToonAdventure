using System.IO;
using Game.Scripts.Core.Levels;
using Game.Scripts.Data;
using Game.Scripts.Utilities.Constants;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core.Save
{
    public class SaveManager
    {
        public UnityEvent<float> OnMusicVolumeChanged { get; } = new();
        public UnityEvent<float> OnSfxVolumeChanged { get; } = new();
        public UnityEvent<LevelType> OnLevelChanged { get; } = new();

        private GameProgressData _progressData;
        private string _jsonPath;

        public float MusicVolume
        {
            get => PlayerPrefs.GetFloat(SaveKeys.MUSIC_VOLUME, 0.3f);
            set
            {
                PlayerPrefs.SetFloat(SaveKeys.MUSIC_VOLUME, Mathf.Clamp01(value));
                PlayerPrefs.Save();
                OnMusicVolumeChanged.Invoke(value);
            }
        }

        public float SfxVolume
        {
            get => PlayerPrefs.GetFloat(SaveKeys.SFX_VOLUME, 0.3f);
            set
            {
                PlayerPrefs.SetFloat(SaveKeys.SFX_VOLUME, Mathf.Clamp01(value));
                PlayerPrefs.Save();
                OnSfxVolumeChanged.Invoke(value);
            }
        }

        public LevelType CurrentLevelId
        {
            get => _progressData.CurrentLevelId;
            set
            {
                if (_progressData.CurrentLevelId == value) return;
                
                _progressData.CurrentLevelId = value;
                SaveProgress();
                OnLevelChanged.Invoke(value);
            }
        }
        
        public void InitialLoad()
        {
            _jsonPath = Path.Combine(Application.persistentDataPath, "game_progress.json");
            LoadProgress();
            
            OnMusicVolumeChanged.Invoke(MusicVolume);
            OnSfxVolumeChanged.Invoke(SfxVolume);
            OnLevelChanged.Invoke(CurrentLevelId);
        }

        private void SaveProgress()
        {
            try
            {
                string json = JsonUtility.ToJson(_progressData, true);
                File.WriteAllText(_jsonPath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save progress: {e.Message}");
            }
        }

        private void LoadProgress()
        {
            if (!File.Exists(_jsonPath))
            {
                _progressData = new GameProgressData();
                SaveProgress();
                return;
            }

            try
            {
                string json = File.ReadAllText(_jsonPath);
                _progressData = JsonUtility.FromJson<GameProgressData>(json) ?? new GameProgressData();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to load progress: {e.Message}");
                _progressData = new GameProgressData();
            }
        }
    }
}