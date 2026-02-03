using System;
using DG.Tweening;
using Game.Scripts.Setups.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI.Controls
{
    [RequireComponent(typeof(Image))]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField, Space] private Image _buttonImage;
        [SerializeField] private TMP_Text _buttonText;
        
        [SerializeField, Space] private Vector3AnimationProperty _hoverProperty;
        [SerializeField] private Vector3AnimationProperty _clickProperty;
        [SerializeField] private Vector3AnimationProperty _idleProperty;
        [SerializeField] private Vector3AnimationProperty _disableProperty;
        
        private Tween _hoverTween;
        private Tween _clickTween;
        private Tween _idleTween;
        private Tween _disableTween;
        
        private bool _isActive = true;
        
        public UnityEvent OnClick { get; private set; } = new UnityEvent();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            
            KillTweens();
            
            _hoverTween = transform
                .DOScale(_hoverProperty.Target, _hoverProperty.Duration)
                .SetEase(_hoverProperty.Curve);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            
            KillTweens();
            
            _clickTween = transform
                .DOScale(_clickProperty.Target, _clickProperty.Duration)
                .SetEase(_clickProperty.Curve);
            
            OnClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            
            KillTweens();
            
            _hoverTween = transform
                .DOScale(_hoverProperty.Target, _hoverProperty.Duration)
                .SetEase(_hoverProperty.Curve);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isActive)
                return;
            
            KillTweens();

            _idleTween = transform
                .DOScale(_idleProperty.Target, _idleProperty.Duration)
                .SetEase(_idleProperty.Curve);;
        }
        
        public void SetActiveButton(bool isActive)
        {
            if (_isActive == isActive)
                return;
            
            KillTweens();
            
            _isActive = isActive;

            if (_isActive)
            {
                var idleSequence = DOTween.Sequence();
            
                var idleScaleTween = transform
                    .DOScale(_idleProperty.Target, _idleProperty.Duration)
                    .SetEase(_idleProperty.Curve);
            
                var idleImageFadeTween = _buttonImage
                    .DOFade(1f, _idleProperty.Duration)
                    .SetEase(_idleProperty.Curve);
            
                var idleTextFadeTween = _buttonText
                    .DOFade(1f, _idleProperty.Duration)
                    .SetEase(_idleProperty.Curve);
            
                idleSequence.Join(idleScaleTween);
                idleSequence.Join(idleImageFadeTween);
                idleSequence.Join(idleTextFadeTween);
                
                _idleTween = idleSequence.OnComplete(() =>
                {
                    _buttonImage.color = Color.white;
                    _buttonText.color = Color.white;
                });
                
                return;
            }
            
            var disableSequence = DOTween.Sequence();
            
            var disableScaleTween = transform
                .DOScale(_disableProperty.Target, _disableProperty.Duration)
                .SetEase(_disableProperty.Curve);
            
            var disableImageFadeTween = _buttonImage
                .DOFade(0.5f, _disableProperty.Duration)
                .SetEase(_disableProperty.Curve);
            
            var disableTextFadeTween = _buttonText
                .DOFade(0.5f, _disableProperty.Duration)
                .SetEase(_disableProperty.Curve);
            
            disableSequence.Join(disableScaleTween);
            disableSequence.Join(disableImageFadeTween);
            disableSequence.Join(disableTextFadeTween);

            _disableTween = disableSequence.OnComplete(() =>
            {
                _buttonImage.color = new Color(1f, 1f, 1f, 0.5f);
                _buttonText.color = new Color(1f, 1f, 1f, 0.5f);
            });
        }

        private void KillTweens()
        {
            _hoverTween?.Kill();
            _clickTween?.Kill();
            _idleTween?.Kill();
            _disableTween?.Kill();
        }

        private void OnDestroy()
        {
            OnClick?.RemoveAllListeners();
        }
    }
}