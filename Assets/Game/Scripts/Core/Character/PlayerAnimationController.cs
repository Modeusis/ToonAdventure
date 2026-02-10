using System;
using Game.Scripts.Core.Interactions;
using UnityEngine;

namespace Game.Scripts.Core.Character
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField, Space] private Animator _animator;
        
        private static readonly int SpeedKey = Animator.StringToHash("Speed");
        private static readonly int VictoryKey = Animator.StringToHash("Victory");
        
        private static readonly int InteractionDownKey = Animator.StringToHash("InteractionDown");
        private static readonly int InteractionKey = Animator.StringToHash("Interaction");

        public void SetSpeed(float speedNormalized)
        {
            _animator.SetFloat(SpeedKey, speedNormalized);
        }

        public void TriggerInteraction(InteractionAnimationType animationType)
        {
            switch (animationType)
            {
                case InteractionAnimationType.None:
                    return;
                case InteractionAnimationType.Normal:
                    _animator.SetTrigger(InteractionKey);
                    break;
                case InteractionAnimationType.Down:
                    _animator.SetTrigger(InteractionDownKey);
                    break;
            }
        }
        
        public void TriggerVictory()
        {
            _animator.SetTrigger(VictoryKey);
        }
    }
}