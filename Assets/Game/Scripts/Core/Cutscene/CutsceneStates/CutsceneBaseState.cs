using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Audio;
using Game.Scripts.Utilities.StateMachine;
using UnityEngine;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public abstract class CutsceneBaseState : State<CutsceneState>, IDisposable
    {
        protected readonly CutsceneConfig Config;
        protected readonly CutsceneActors Actors;
        protected readonly Action OnCompleteCallback;
        
        private CancellationTokenSource _stateCts;
        private float _timer;
        private bool _isCompleted;

        protected CutsceneBaseState(CutsceneConfig config, CutsceneActors actors, Action onComplete)
        {
            StateType = config.State;
            Config = config;
            Actors = actors;
            OnCompleteCallback = onComplete;
        }

        public override void Enter()
        {
            _timer = 0f;
            _isCompleted = false;
            _stateCts = new CancellationTokenSource();

            if (Config.VirtualCamera != null)
            {
                Debug.Log("[Enter] New camera set");
                Config.VirtualCamera.gameObject.SetActive(true);
            }
            
            if (Config.Sound != SoundType.CutsceneNone)
                G.Audio.PlaySfx(Config.Sound);

            ExecuteAsync(_stateCts.Token).Forget();
        }

        protected abstract UniTask ExecuteAsync(CancellationToken token);

        public override void Update()
        {
            if (_isCompleted) return;

            _timer += Time.deltaTime;

            if (Config.Duration > 0 && _timer >= Config.Duration)
            {
                Complete();
            }
        }

        protected void Complete()
        {
            if (_isCompleted) return;
            _isCompleted = true;
            OnCompleteCallback?.Invoke();
        }

        public override void Exit()
        {
            DisposeToken();
            
            if (Config.VirtualCamera != null)
                Config.VirtualCamera.gameObject.SetActive(false);
            
            OnExit();
        }
        
        protected virtual void OnExit() { }
        
        public void Dispose()
        {
            DisposeToken();
        }
        
        private void DisposeToken()
        {
            if (_stateCts != null)
            {
                _stateCts.Cancel();
                _stateCts.Dispose();
                _stateCts = null;
            }
        }
    }
}