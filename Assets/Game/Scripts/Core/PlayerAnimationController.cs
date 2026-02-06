using UnityEngine;

namespace Game.Scripts.Core
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField, Space] private Animator _animator;
        
        private static readonly int SpeedKey = Animator.StringToHash("Speed");
        private static readonly int VictoryKey = Animator.StringToHash("Victory");

        public void SetSpeed(float speedNormalized)
        {
            _animator.SetFloat(SpeedKey, speedNormalized);
        }

        public void TriggerVictory()
        {
            _animator.SetTrigger(VictoryKey);
        }
    }
}