using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.NPC;
using Random = UnityEngine.Random;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class OutroState : CutsceneBaseState
    {
        private readonly Action _onFinalCutsceneEnd;

        public OutroState(CutsceneConfig config, CutsceneActors actors, Action onComplete, Action onFinalCutsceneEnd) 
            : base(config, actors, onComplete) 
        {
            _onFinalCutsceneEnd = onFinalCutsceneEnd;
        }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            RandomLoopAnimation(Actors.Player, token).Forget();
            RandomLoopAnimation(Actors.Adam, token).Forget();
            RandomLoopAnimation(Actors.Maks, token).Forget();
            
            await UniTask.Delay(TimeSpan.FromSeconds(Config.Duration), cancellationToken: token);
            
            _onFinalCutsceneEnd?.Invoke();
        }

        private async UniTaskVoid RandomLoopAnimation(NpcAnimationController npc, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                npc.PlayAction();
                var delay = Random.Range(1.5f, 3.5f);
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }
        }

        protected override void OnExit()
        {
            Actors.StopLeoPatrol();
        }
    }
}