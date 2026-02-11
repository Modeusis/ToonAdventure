using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class NoneState : CutsceneBaseState
    {
        public NoneState(CutsceneConfig config, CutsceneActors actors, Action onComplete) 
            : base(config, actors, onComplete) { }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            await UniTask.Yield();
        }
    }
}