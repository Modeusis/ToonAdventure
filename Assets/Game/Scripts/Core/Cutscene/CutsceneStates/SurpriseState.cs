using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.NPC;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class SurpriseState : CutsceneBaseState
    {
        public SurpriseState(CutsceneConfig config, CutsceneActors actors, Action onComplete) 
            : base(config, actors, onComplete) { }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            LoopAnimation(Actors.Adam, 2.0f, token).Forget();
            LoopAnimation(Actors.Maks, 2.5f, token).Forget();
            
            await UniTask.CompletedTask;
        }

        private async UniTaskVoid LoopAnimation(NpcAnimationController npc, float interval, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                npc.PlayAction();
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: token);
            }
        }
    }
}