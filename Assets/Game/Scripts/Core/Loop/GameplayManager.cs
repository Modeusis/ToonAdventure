using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Character;
using Game.Scripts.Core.Levels;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.UI.Screens.Dialog;
using Game.Scripts.Utilities.Events;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Core.Loop
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField, Space] private LevelManager _levelManager;
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private QuestManager _questManager;
        
        [SerializeField] private CinemachineBrain _brain;
        
        [SerializeField, Space] private GameObject _playerPrefab;
        
        private Player _player;
        
        private void Start()
        {
            _levelManager.Initialize();
            _dialogueManager.Initialize();
            _questManager.Initialize();
            
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
            
            G.Camera.SetBrain(_brain);
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
            playerObject.name = "[Player]";    
            
            _player = playerObject.GetComponent<Player>();
            _player.Initialize();
            
            _player.Sfx.SetFootstepType(level.StepType);
            
            _questManager.StartQuest(level.QuestId);
            
            G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Active });
            G.UI.Screens.HUD.DynamicTooltip.HideFast();
            G.Save.CurrentLevelId = level.Type;
        }
    }
}