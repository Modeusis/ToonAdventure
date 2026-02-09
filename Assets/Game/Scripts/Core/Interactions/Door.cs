using DG.Tweening;
using Game.Scripts.Setups.Animations;
using UnityEngine;

namespace Game.Scripts.Core.Interactions
{
    public class Door : MonoBehaviour
    {
        [SerializeField, Space] private Transform _doorTransform;
        
        [SerializeField, Space] private FloatAnimationProperty _rotationOpenAnimation;
        [SerializeField] private FloatAnimationProperty _rotationCloseAnimation;
        
        [SerializeField, Space] private bool _isOpenOnStart;
        
        private Collider _collider;
        
        private Transform _targetTransform;
        private Transform _lastInteractor;
        
        private Tween _openTween;
        private bool _isOpen;

        private void Awake()
        {
            _collider = _doorTransform.GetComponent<Collider>();
            _targetTransform = _doorTransform != null ? _doorTransform : transform;
        }
        
        private void Start()
        {
            if (_isOpenOnStart)
            {
                _targetTransform.localRotation = Quaternion.Euler(0, _rotationOpenAnimation.Target, 0);
                _isOpen = true;
                
                return;
            }
            
            _targetTransform.localRotation = Quaternion.Euler(0, _rotationCloseAnimation.Target, 0);
            _isOpen = false;
        }

        public void SetInteractor(Transform interactor)
        {
            _lastInteractor = interactor;
        }
        
        public void Toggle()
        {
            _openTween?.Kill();
            _openTween = null;

            _collider.enabled = false;

            float targetAngle;
            float duration;
            AnimationCurve curve;

            if (_isOpen)
            {
                targetAngle = _rotationCloseAnimation.Target;
                duration = _rotationCloseAnimation.Duration;
                curve = _rotationCloseAnimation.Curve;
            }
            else
            {
                targetAngle = CalculateOpenAngle();
                duration = _rotationOpenAnimation.Duration;
                curve = _rotationOpenAnimation.Curve;
            }
            
            _openTween = _targetTransform
                .DOLocalRotate(Vector3.up * targetAngle, duration)
                .SetEase(curve)
                .OnComplete(() =>
                {
                    _collider.enabled = true;
                });
            
            _isOpen = !_isOpen;
        }

        private float CalculateOpenAngle()
        {
            var baseAngle = Mathf.Abs(_rotationOpenAnimation.Target);

            if (_lastInteractor == null)
                return baseAngle;

            var directionToInteractor = _lastInteractor.position - _targetTransform.position;
            var dot = Vector3.Dot(_targetTransform.forward, directionToInteractor);
            
            return dot > 0 ? -baseAngle : baseAngle;
        }
    }
}