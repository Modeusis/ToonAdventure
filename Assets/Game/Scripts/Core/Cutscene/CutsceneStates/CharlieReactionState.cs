using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.NPC;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class CharlieReactionState : CutsceneBaseState
    {
        public CharlieReactionState(CutsceneConfig config, CutsceneActors actors, Action onComplete) 
            : base(config, actors, onComplete) { }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            var playerNav = Actors.Player.GetComponent<NpcNavigator>();
            
            Actors.Player.PlayAction();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: token);
            
            Actors.Player.PlayAction();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: token);
            
            await Actors.MoveToAsync(playerNav, Actors.CakePoint.position, token);
        }
    }
}