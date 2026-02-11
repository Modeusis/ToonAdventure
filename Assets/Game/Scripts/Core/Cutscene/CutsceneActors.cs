using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.NPC;
using UnityEngine;

namespace Game.Scripts.Core.Cutscene
{
    public class CutsceneActors : MonoBehaviour
    {
        [Header("Characters")]
        [field: SerializeField] public NpcAnimationController Leo { get; private set; }
        [field: SerializeField] public NpcAnimationController Player { get; private set; }
        [field: SerializeField] public NpcAnimationController Adam { get; private set; }
        [field: SerializeField] public NpcAnimationController Maks { get; private set; }

        [Header("Navigation Targets")]
        [field: SerializeField] public Transform LeoStartPoint { get; private set; }
        [field: SerializeField] public Transform PlayerStartPoint { get; private set; }
        [field: SerializeField] public Transform LeoPatrolPointA { get; private set; }
        [field: SerializeField] public Transform LeoPatrolPointB { get; private set; }
        [field: SerializeField] public Transform CakePoint { get; private set; }
        
        [Header("Look Targets")]
        [field: SerializeField] public Transform PatrolLookA { get; private set; }
        [field: SerializeField] public Transform PatrolLookB { get; private set; }

        private CancellationTokenSource _leoPatrolCts;

        private void OnDestroy()
        {
            StopLeoPatrol();
        }
        
        public void StartLeoPatrol()
        {
            StopLeoPatrol();
            _leoPatrolCts = new CancellationTokenSource();
            LeoPatrolRoutine(_leoPatrolCts.Token).Forget();
        }

        public void StopLeoPatrol()
        {
            if (_leoPatrolCts != null)
            {
                _leoPatrolCts.Cancel();
                _leoPatrolCts.Dispose();
                _leoPatrolCts = null;
            }
            
            if (Leo != null)
            {
                var navigator = Leo.GetComponent<NpcNavigator>();
                if (navigator != null) navigator.Stop();
            }
        }

        private async UniTaskVoid LeoPatrolRoutine(CancellationToken token)
        {
            var navigator = Leo.GetComponent<NpcNavigator>();
            
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await MoveToAsync(navigator, LeoPatrolPointA.position, token);
                    navigator.SetLookTarget(PatrolLookA);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    Leo.PlayAction();
                    await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    
                    await MoveToAsync(navigator, LeoPatrolPointB.position, token);
                    navigator.SetLookTarget(PatrolLookB);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    Leo.PlayAction();
                    await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
                    
                    navigator.SetLookTarget(PatrolLookA);
                }
            }
            catch (OperationCanceledException) { }
        }
        
        public async UniTask MoveToAsync(NpcNavigator navigator, Vector3 target, CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource();
            
            navigator.MoveTo(target, () => completionSource.TrySetResult());
            
            using (token.Register(() => completionSource.TrySetCanceled()))
            {
                await completionSource.Task;
            }
        }
    }
}