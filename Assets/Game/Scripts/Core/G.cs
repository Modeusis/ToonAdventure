using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Audio;
using Game.Scripts.UI.Root;
using Game.Scripts.Utilities.Constants;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.Load;
using UnityEngine;

namespace Game.Scripts.Core
{
    [DefaultExecutionOrder(-1000)]
    public class G : MonoBehaviour
    {
        private static G _instance;

        public static EventBus EventBus => _instance?._eventBus;
        public static AddressableLoader Loader => _instance?._loader;
        public static AudioManager Audio => _instance?._audioManager;
        public static UiRoot UI => _instance?._uiRoot;
        
        public static bool IsReady => _instance != null && _instance._isInitialized;

        private EventBus _eventBus;
        private AddressableLoader _loader;
        private AudioManager _audioManager;
        private UiRoot _uiRoot;
        
        private bool _isInitialized;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGameAsync().Forget();
        }

        private async UniTaskVoid InitializeGameAsync()
        {
            _eventBus = new EventBus();
            _loader = new AddressableLoader();

            var audioTask = _loader.InstantiateAsync<AudioManager>(Addresses.AUDIO_SERVICE, instanceName: "[Audio]");
            var uiTask = _loader.InstantiateAsync<UiRoot>(Addresses.UI_ROOT, instanceName: "[UI]");
            
            var (loadedAudio, loadedUi) = await UniTask.WhenAll(audioTask, uiTask);

            if (!loadedAudio || !loadedUi)
            {
                Debug.LogError("[G] Failed to load core systems via Addressables");
                return;
            }

            _audioManager = loadedAudio;
            DontDestroyOnLoad(_audioManager.gameObject);
            
            _uiRoot = loadedUi;
            _uiRoot.Initialize();
            DontDestroyOnLoad(_uiRoot.gameObject);
            
            _isInitialized = true;
            
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
    }
}