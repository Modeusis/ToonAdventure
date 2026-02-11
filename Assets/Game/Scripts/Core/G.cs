using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Audio;
using Game.Scripts.Core.Cameras;
using Game.Scripts.Core.Cameras.Cursors;
using Game.Scripts.Core.Save;
using Game.Scripts.Core.Scenes;
using Game.Scripts.Setups.Core;
using Game.Scripts.UI.Root;
using Game.Scripts.UI.Screens.Dialog;
using Game.Scripts.Utilities.Constants;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.Load;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Scripts.Core
{
    public class G : IDisposable
    {
        private static G _instance;

        public static EventBus EventBus => _instance?._eventBus;
        public static BaseInput Input => _instance?._input;
        public static AddressableLoader Loader => _instance?._loader;
        public static SceneLoader Scenes => _instance?._sceneLoader;
        public static AudioManager Audio => _instance?._audioManager;
        public static UiRoot UI => _instance?._uiRoot;
        public static SaveManager Save => _instance?._saveManager;
        public static CameraManager Camera => _instance?._cameraManager;
        public static CursorManager Cursor => _instance?._cursorManager;
        
        public static bool IsReady => _instance != null && _instance._isInitialized;
        public static bool IsTestMode => _instance != null && !_instance._isStandardStart;
        
        private readonly EventBus _eventBus;
        private readonly AddressableLoader _loader;
        
        private BaseInput _input;
        private SceneLoader _sceneLoader;
        private AudioManager _audioManager;
        private UiRoot _uiRoot;
        private SaveManager _saveManager;
        private CameraManager _cameraManager;
        private CursorManager _cursorManager;
        
        private GameManagerSetup _setup;

        private bool _isStandardStart = true;
        private bool _isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_instance != null)
                return;

            _instance = new G();
            _instance.RunGameAsync().Forget();
        }

        private G()
        {
            _loader = new AddressableLoader();
            _eventBus = new EventBus();
        }
        
        private async UniTask RunGameAsync()
        {
            _setup = await _loader.LoadAsync<GameManagerSetup>(Addresses.G_SETUP_KEY);
            if (!_setup)
            {
                Debug.LogError("[G] Failed to load game manager setup via Addressables");
                return;
            }
            
#if UNITY_EDITOR
            var currentSceneName = SceneManager.GetActiveScene().name;
            
            var isLoadingScene = currentSceneName == SceneNames.BOOT
                                  || currentSceneName == SceneNames.MAIN_MENU
                                  || currentSceneName == SceneNames.GAMEPLAY
                                  || currentSceneName == SceneNames.CUTSCENE;

            _isStandardStart = isLoadingScene;
#endif
            if (_isStandardStart)
            {
                Debug.Log("[G] Loading game standard start");
                
                await _loader.LoadSceneAsync(Addresses.BOOT_SCENE_KEY);
                await _loader.LoadSceneAsync(Addresses.MAIN_MENU_SCENE_KEY);
            }
            
            await LoadAndRunGame();
        }

        private async UniTask LoadAndRunGame()
        {
            _input = new BaseInput();
            _input.Enable();
            
            _saveManager = new SaveManager();
            _sceneLoader = new SceneLoader(_setup.LoadDelay);
            _cursorManager = new CursorManager(_setup.StartCursorState);
            
            var audioTask = _loader.InstantiateAsync<AudioManager>(Addresses.AUDIO_MANAGER_KEY, instanceName: "[Audio]");
            var uiTask = _loader.InstantiateAsync<UiRoot>(Addresses.UI_ROOT_KEY, instanceName: "[UI]");
            
            var (loadedAudio, loadedUi) = await UniTask.WhenAll(audioTask, uiTask);

            if (!loadedAudio || !loadedUi)
            {
                Debug.LogError("[G] Failed to load core systems via Addressables");
                return;
            }

            _cameraManager = new CameraManager(_setup.CameraSettings);
            
            _audioManager = loadedAudio;
            _audioManager.Initialize();
            Object.DontDestroyOnLoad(_audioManager.gameObject);
            
            _uiRoot = loadedUi;
            _uiRoot.Initialize();
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
            
            _isInitialized = true;
            
            _saveManager.InitialLoad();
            
            _eventBus.Publish(new OnGameReadyEvent());
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _eventBus?.Dispose();
                _loader?.Dispose();
                _instance = null;
            }
        }

        public void Dispose()
        {
            _eventBus?.Dispose();
            _input?.Dispose();
            _loader?.Dispose();
            
            _instance = null;
        }
    }
}