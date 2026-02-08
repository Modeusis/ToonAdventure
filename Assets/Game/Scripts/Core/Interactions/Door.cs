using DG.Tweening;
using Game.Scripts.Setups.Animations;
using UnityEngine;

namespace Game.Scripts.Core.Interactions
{
    [RequireComponent(typeof(Collider))]
    public class Door : MonoBehaviour
    {
        [SerializeField, Space] private Transform _doorTransform;
        
        [SerializeField, Space] private FloatAnimationProperty _rotationOpenAnimation;
        [SerializeField] private FloatAnimationProperty _rotationCloseAnimation;
        
        [SerializeField, Space] private bool _isOpenOnStart;
        
        private Collider _collider;
        
        private Tween _openTween;
        private bool _isOpen;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            
            if (_isOpenOnStart)
            {
                transform.Rotate(Vector3.up, _rotationOpenAnimation.Target);
                _isOpen = true;
                
                return;
            }
            
            transform.Rotate(Vector3.up, _rotationCloseAnimation.Target);
            _isOpen = false;
        }

        public void Toggle()
        {
            _openTween?.Kill();
            _openTween = null;

            _collider.enabled = false;
            
            var rotation = Vector3.up * (_isOpen ? _rotationCloseAnimation.Target : _rotationOpenAnimation.Target);
            var duration = _isOpen ? _rotationCloseAnimation.Duration : _rotationOpenAnimation.Duration;
            var curve = _isOpen ? _rotationCloseAnimation.Curve : _rotationOpenAnimation.Curve;
            
            _openTween = transform
                .DOLocalRotate(rotation, duration)
                .SetEase(curve)
                .OnComplete(() =>
                {
                    _collider.enabled = true;
                });
            
            _isOpen = !_isOpen;
        }
    }
}