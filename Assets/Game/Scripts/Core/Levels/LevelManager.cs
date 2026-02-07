using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Audio;
using Game.Scripts.Setups.Levels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Core.Levels
{
    public struct OnLevelLoadedEvent 
    {
        public Level LoadedLevel;
    }
    
    public class LevelManager : MonoBehaviour
    {
        [SerializeField, Space] private LevelSetup _setup;
        [SerializeField, Space] private Transform _levelRoot;
        
        private Dictionary<LevelType, LevelInfo> _mappedLevels;

        private Level _currentLevel;
        
        private bool _isInitialized;
        
        public void Initialize()
        {
            if (!_setup.TryMapDictionary(out _mappedLevels))
            {
                Debug.LogWarning("[MenuRoot.Awake] Error while trying to map dictionary, levels are not initialized.");
                return;
            }

            _isInitialized = true;
            
            G.EventBus.Subscribe<LevelType>(StartLoadingLevel);
        }

        private void StartLoadingLevel(LevelType levelType)
        {
            LoadLevel(levelType).Forget();
        }
        
        private async UniTask LoadLevel(LevelType levelType)
        {
            if (!_isInitialized)
                return;
            
            if (!_mappedLevels.TryGetValue(levelType, out var levelInfo))
            {
                Debug.LogWarning($"[LevelManager.Load] Level {levelType} is not mapped.");
                return;
            }

            if (_currentLevel != null)
            {
                G.UI.Loading.Show("Выгружаем уровень...");

                await UniTask.WaitForSeconds(_setup.LevelLoadDelay);
                
                _currentLevel.Unload();
                Destroy(_currentLevel.gameObject);
                _currentLevel = null;
            }
            
            G.UI.Loading.Show($"Загружаем {levelInfo.LoadTag}");
            
            await UniTask.WaitForSeconds(_setup.LevelLoadDelay);
            
            _currentLevel = Instantiate(levelInfo.Prefab, transform);
            _currentLevel.Load();

            if (_currentLevel.MusicType != MusicType.None)
            {
                G.Audio.PlayMusic(_currentLevel.MusicType);
            }
            
            G.EventBus.Publish(new OnLevelLoadedEvent { LoadedLevel = _currentLevel });
        }

        private void OnDestroy()
        {
            if (!G.IsReady)
                return;
            
            G.EventBus.Unsubscribe<LevelType>(StartLoadingLevel);
        }
    }
}