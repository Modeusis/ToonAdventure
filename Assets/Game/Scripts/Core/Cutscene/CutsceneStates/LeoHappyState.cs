using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class LeoHappyState : CutsceneBaseState
    {
        public LeoHappyState(CutsceneConfig config, CutsceneActors actors, Action onComplete) 
            : base(config, actors, onComplete) { }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            Actors.StartLeoPatrol();
            await UniTask.CompletedTask;
        }
    }
}