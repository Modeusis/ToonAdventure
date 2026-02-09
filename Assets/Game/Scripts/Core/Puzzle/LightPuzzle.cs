using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Character;
using Game.Scripts.Utilities.Events;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core.Puzzle
{
    public class LightPuzzle : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _puzzleCamera;
        [SerializeField] private Renderer[] _lights;
        
        [Header("Materials")]
        [SerializeField] private Material _offMaterial;
        [SerializeField] private Material _onMaterial;
        [SerializeField] private Material _errorMaterial;
        
        [Header("Settings")]
        [SerializeField] private float _flashDuration = 0.5f;
        [SerializeField] private float _delayBetweenFlashes = 0.2f;

        private List<int> _sequence = new List<int>();
        private int _currentWave = 0;
        private int _inputIndex = 0;
        private bool _isInputActive;
        
        private CancellationTokenSource _puzzleCts;

        private void Start()
        {
            _puzzleCamera.Priority = 0;
            ResetLights();
        }

        private void OnDestroy()
        {
            StopPuzzle();
        }

        public void BeginPuzzle()
        {
            G.EventBus.Publish(new OnPuzzleBeginEvent());
            G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Puzzle });
            
            _puzzleCamera.Priority = 20;
            
            _puzzleCts?.Cancel();
            _puzzleCts = new CancellationTokenSource();

            _currentWave = 1;
            RunGameLoop(_puzzleCts.Token).Forget();
        }

        public void StopPuzzle()
        {
            _puzzleCts?.Cancel();
            _puzzleCts?.Dispose();
            _puzzleCts = null;
            
            _isInputActive = false;
            _puzzleCamera.Priority = 0;
            ResetLights();
        }

        public void OnButtonInput(int id)
        {
            if (!_isInputActive) return;

            HandleInputAsync(id).Forget();
        }

        private async UniTaskVoid HandleInputAsync(int id)
        {
            _isInputActive = false; 
            
            await FlashLightAsync(id, _flashDuration / 2, _puzzleCts.Token);

            if (_sequence[_inputIndex] == id)
            {
                _inputIndex++;
                if (_inputIndex >= _sequence.Count)
                {
                    _currentWave++;
                    if (_currentWave > 3)
                    {
                        CompletePuzzle();
                    }
                    else
                    {
                        RunGameLoop(_puzzleCts.Token).Forget();
                    }
                    return;
                }
                
                _isInputActive = true;
            }
            else
            {
                await ShowErrorAsync(_puzzleCts.Token);
                RunGameLoop(_puzzleCts.Token).Forget();
            }
        }

        private async UniTaskVoid RunGameLoop(CancellationToken token)
        {
            _isInputActive = false;
            
            await UniTask.Delay(1000, cancellationToken: token);
            
            GenerateSequence();
            await PlaySequenceAsync(token);
            
            _inputIndex = 0;
            _isInputActive = true;
        }

        private void GenerateSequence()
        {
            _sequence.Clear();
            int count = _currentWave == 1 ? 2 : (_currentWave == 2 ? 4 : 6);
            
            for (int i = 0; i < count; i++)
            {
                _sequence.Add(Random.Range(0, _lights.Length));
            }
        }

        private async UniTask PlaySequenceAsync(CancellationToken token)
        {
            foreach (var index in _sequence)
            {
                await FlashLightAsync(index, _flashDuration, token);
                await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenFlashes), cancellationToken: token);
            }
        }

        private async UniTask FlashLightAsync(int index, float duration, CancellationToken token)
        {
            if (index < 0 || index >= _lights.Length) return;

            _lights[index].material = _onMaterial;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
            _lights[index].material = _offMaterial;
        }
        
        private async UniTask ShowErrorAsync(CancellationToken token)
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (var l in _lights) l.material = _errorMaterial;
                await UniTask.Delay(200, cancellationToken: token);
                foreach (var l in _lights) l.material = _offMaterial;
                await UniTask.Delay(200, cancellationToken: token);
            }
        }

        private void ResetLights()
        {
            foreach (var l in _lights)
            {
                if(l) l.material = _offMaterial;
            }
        }

        private void CompletePuzzle()
        {
            G.EventBus.Publish(new OnPuzzleSolvedEvent());
            G.EventBus.Publish(new OnPlayerStateChangeRequest { NewState = PlayerState.Active });
            StopPuzzle();
        }
    }
}