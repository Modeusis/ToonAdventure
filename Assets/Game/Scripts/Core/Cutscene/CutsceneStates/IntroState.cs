using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.NPC;

namespace Game.Scripts.Core.Cutscene.CutsceneStates
{
    public class IntroState : CutsceneBaseState
    {
        public IntroState(CutsceneConfig config, CutsceneActors actors, Action onComplete) 
            : base(config, actors, onComplete) { }

        protected override async UniTask ExecuteAsync(CancellationToken token)
        {
            var leoNav = Actors.Leo.GetComponent<NpcNavigator>();
            var playerNav = Actors.Player.GetComponent<NpcNavigator>();

            var taskLeo = Actors.MoveToAsync(leoNav, Actors.LeoStartPoint.position, token);
            var taskPlayer = Actors.MoveToAsync(playerNav, Actors.PlayerStartPoint.position, token);

            await UniTask.WhenAll(taskLeo, taskPlayer);
        }
    }
}