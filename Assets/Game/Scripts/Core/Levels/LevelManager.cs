using System.Collections.Generic;
using Game.Scripts.Core.Audio;
using Game.Scripts.Setups.Levels;
using UnityEngine;

namespace Game.Scripts.Core.Levels
{
    public struct OnLevelLoadedEvent 
    {
        public Levels.Level LoadedLevel;
    }
    
    public class LevelManager : MonoBehaviour
    {
        [SerializeField, Space] private LevelSetup _levels;
        [SerializeField, Space] private Transform _levelRoot;
        
        private Dictionary<LevelType, Levels.Level> _mappedLevels;

        private Levels.Level _currentLevel;
        
        private bool _isInitialized;
        
        public void Initialize()
        {
            if (!_levels.TryMapDictionary(out _mappedLevels))
            {
                Debug.LogWarning("[MenuRoot.Awake] Error while trying to map dictionary, levels are not initialized.");
                return;
            }

            _isInitialized = true;
            
            G.EventBus.Subscribe<LevelType>(LoadLevel);
        }

        private void LoadLevel(LevelType levelType)
        {
            if (!_isInitialized)
                return;
            
            if (!_mappedLevels.TryGetValue(levelType, out var newLevelPrefab))
            {
                Debug.LogWarning($"[LevelManager.Load] Level {levelType} is not mapped.");
                return;
            }

            if (_currentLevel != null)
            {
                _currentLevel.Unload();
                Destroy(_currentLevel.gameObject);
                _currentLevel = null;
            }
            
            _currentLevel = Instantiate(newLevelPrefab, transform);
            _currentLevel.Load();
            
            Debug.Log($"[LevelManager.LoadLevel] Level {levelType} loaded.");

            if (_currentLevel.MusicType != MusicType.None)
            {
                G.Audio.PlayMusic(_currentLevel.MusicType);
            }
            
            G.EventBus.Publish(new OnLevelLoadedEvent { LoadedLevel = _currentLevel });
        }
    }
}