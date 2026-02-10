using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Loop.Dialogues;
using Game.Scripts.Core.NPC;
using UnityEngine;

namespace Game.Scripts.Core.Npc.Adam
{
    public enum AdamState
    {
        StartState,
        OneToyFound,
        TwoToysFound
    }

    public class AdamKitchen : MonoBehaviour
    {
        [Header("References")]
        [field: SerializeField] public NpcNavigator Navigator { get; private set; }
        [field: SerializeField] public NpcAnimationController Animator { get; private set; }
        [field: SerializeField] public InteractableObject Interactable { get; private set; }

        [Header("Patrol Settings")]
        [SerializeField] private Transform _pointATarget;
        [SerializeField] private Transform _pointALook;
        [SerializeField] private Transform _pointBTarget;
        [SerializeField] private Transform _pointBLook;
        [SerializeField] private float _waitAtPointA = 3f;
        [SerializeField] private float _waitAtPointB = 3f;

        [Header("State Settings")]
        public AdamState CurrentState = AdamState.StartState;
        
        [field: SerializeField, Space] public Dialogue DialogStart { get; private set; }
        [field: SerializeField] public Dialogue DialogOneToy { get; private set; }
        [field: SerializeField] public Dialogue DialogTwoToys { get; private set; }

        private CancellationTokenSource _patrolCts;
        private bool _isInteracting;

        private void Start()
        {
            Interactable.OnInteractionZoneEnter.AddListener(OnPlayerEnterZone);
            Interactable.OnInteractionZoneExit.AddListener(OnPlayerExitZone);
            Interactable.OnInteractionProceed.AddListener(OnInteractionProceed);
            
            StartPatrolLoop();
        }

        private void OnDestroy()
        {
            StopPatrolLoop();
            
            Interactable.OnInteractionZoneEnter.RemoveListener(OnPlayerEnterZone);
            Interactable.OnInteractionZoneExit.RemoveListener(OnPlayerExitZone);
            Interactable.OnInteractionProceed.RemoveListener(OnInteractionProceed);
        }

        #region Patrol Logic

        private void StartPatrolLoop()
        {
            if (_isInteracting) return;
            
            _patrolCts?.Cancel();
            _patrolCts = new CancellationTokenSource();
            
            PatrolRoutine(_patrolCts.Token).Forget();
        }

        private void StopPatrolLoop()
        {
            _patrolCts?.Cancel();
            _patrolCts?.Dispose();
            _patrolCts = null;
            
            if (Navigator != null) 
                Navigator.Stop();
        }

        private async UniTaskVoid PatrolRoutine(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await MoveToAsync(_pointATarget.position, token);
                    Navigator.SetLookTarget(_pointALook);
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitAtPointA), cancellationToken: token);
                    Animator.PlayAction();
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitAtPointA), cancellationToken: token);
                    await MoveToAsync(_pointBTarget.position, token);
                    Navigator.SetLookTarget(_pointBLook);
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitAtPointB), cancellationToken: token);
                    Animator.PlayAction();
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitAtPointB), cancellationToken: token);
                    Navigator.SetLookTarget(_pointALook);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        private async UniTask MoveToAsync(Vector3 target, CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource();
            
            Navigator.MoveTo(target, () =>
            {
                completionSource.TrySetResult();
            });
            
            using (token.Register(() => completionSource.TrySetCanceled()))
            {
                await completionSource.Task;
            }
        }

        #endregion

        #region Interaction Logic
        
        private void OnPlayerEnterZone(Transform playerTransform)
        {
            _isInteracting = true;
            
            StopPatrolLoop();
            
            Navigator.SetLookTarget(playerTransform);
        }
        
        private void OnInteractionProceed()
        {
            PlayDialogByState();
        }
        
        private void OnPlayerExitZone()
        {
            _isInteracting = false;
            
            Navigator.SetLookTarget(null);

            StartPatrolLoop();
        }

        private void PlayDialogByState()
        {
            var dialogToPlay = DialogStart;

            switch (CurrentState)
            {
                case AdamState.StartState:
                    dialogToPlay = DialogStart;
                    break;
                case AdamState.OneToyFound:
                    dialogToPlay = DialogOneToy;
                    break;
                case AdamState.TwoToysFound:
                    dialogToPlay = DialogTwoToys;
                    break;
            }

            dialogToPlay?.StartDialogue();
        }
        
        public void SetState(AdamState newState)
        {
            CurrentState = newState;
        }

        #endregion
    }
}