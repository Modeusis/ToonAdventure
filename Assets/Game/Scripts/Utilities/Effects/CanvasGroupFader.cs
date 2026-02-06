using DG.Tweening;
using Game.Scripts.Setups.Animations;
using UnityEngine;

namespace Game.Scripts.Utilities.Effects
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        [SerializeField, Space] private CanvasGroup _canvasGroup;
        [SerializeField, Space] private FloatAnimationProperty _fadeSettings;

        private Tween _activeTween;

        public bool IsBlockRaycast
        {
            get => _canvasGroup.blocksRaycasts;
            set => _canvasGroup.blocksRaycasts = value;
        }
        
        private void Awake()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeIn() => Play(_fadeSettings.Target);

        public void FadeOut() => Play(0f);

        public void CutIn() => _canvasGroup.alpha = _fadeSettings.Target;
        public void CutOut() => _canvasGroup.alpha = 0f;
        
        public void Play(float targetAlpha)
        {
            _activeTween?.Kill();

            _activeTween = _canvasGroup.DOFade(targetAlpha, _fadeSettings.Duration)
                .SetEase(_fadeSettings.Curve)
                .SetLink(gameObject);
        }

        public void Kill()
        {
            _activeTween?.Kill();
            _activeTween = null;
        }

        private void OnDestroy()
        {
            Kill();
        }
    }
}