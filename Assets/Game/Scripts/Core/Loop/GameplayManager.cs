using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Character;
using Game.Scripts.Core.Levels;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Loop
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField, Space] private LevelManager _levelManager;
        [SerializeField, Space] private GameObject _playerPrefab;
        
        private Player _player;
        
        private void Start()
        {
            _levelManager.Initialize();
            G.EventBus.Subscribe<OnLevelLoadedEvent>(OnLevelLoaded);

            StartGameplay().Forget();
        }

        private void OnDestroy()
        {
            if (!G.IsReady)
                return;
            
            G.EventBus.Unsubscribe<OnLevelLoadedEvent>(OnLevelLoaded);
        }
        
        private async UniTaskVoid StartGameplay()
        {
            await UniTask.Yield();
            
            G.EventBus.Publish(G.Save.CurrentLevelId);
        }
        
        private void OnLevelLoaded(OnLevelLoadedEvent eventData)
        {
            if (_player)
            {
                Destroy(_player.gameObject);
                _player = null;
            }
            
            InitializeLevel(eventData.LoadedLevel);
            
            G.UI.Loading.Hide();
        }

        private void InitializeLevel(Level level)
        {
            var spawnPoint = level.StartPoint;
            
            var playerObject = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);
                
            _player = playerObject.GetComponent<Player>();
            _player.Initialize();

            if (level.StartCamera)
            {
                level.StartCamera.Follow = _player.transform;
            }
            
            G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Active });
            G.Save.CurrentLevelId = level.Type;
        }
    }
}