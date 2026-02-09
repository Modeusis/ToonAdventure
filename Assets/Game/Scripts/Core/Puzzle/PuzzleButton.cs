using DG.Tweening;
using Game.Scripts.Core.Interactions;
using Game.Scripts.Core.Puzzle;
using Game.Scripts.Setups.Animations;
using UnityEngine;

namespace Game.Scripts.Gameplay.Puzzles
{
    public class PuzzleButton : MonoBehaviour, IClickable
    {
        [Header("Logic")]
        [SerializeField] private int _id;
        [SerializeField] private LightPuzzle _puzzleController;
        
        [Header("Visual Components")]
        [SerializeField] private Renderer _renderer;

        [Header("Animation Settings")]
        [SerializeField] private FloatAnimationProperty _hoverScale;
        [SerializeField] private FloatAnimationProperty _clickPunchScale;
        [SerializeField] private ColorAnimationProperty _hoverColor;
        [SerializeField] private ColorAnimationProperty _pressColor;

        private Color _defaultColor;
        private Vector3 _defaultScale;
        private Tween _colorTween;
        private Tween _scaleTween;

        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            
            _defaultColor = _renderer.material.color;
            _defaultScale = transform.localScale;
        }

        public void OnBeginHover()
        {
            AnimateScale(_defaultScale * _hoverScale.Value, _hoverScale.Duration, _hoverScale.Ease);
            AnimateColor(_hoverColor.Value, _hoverColor.Duration, _hoverColor.Ease);
        }

        public void OnEndHover()
        {
            AnimateScale(_defaultScale, _hoverScale.Duration, _hoverScale.Ease);
            AnimateColor(_defaultColor, _hoverColor.Duration, _hoverColor.Ease);
        }

        public void OnClick()
        {
            _scaleTween?.Kill(true);
            transform.DOPunchScale(Vector3.one * _clickPunchScale.Value, _clickPunchScale.Duration, 10, 1);

            AnimateColor(_pressColor.Value, _pressColor.Duration, _pressColor.Ease, true);
            
            _puzzleController.OnButtonInput(_id);
        }

        private void AnimateScale(Vector3 targetScale, float duration, AnimationCurve ease)
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(targetScale, duration).SetEase(ease);
        }

        private void AnimateColor(Color targetColor, float duration, AnimationCurve ease, bool autoRevert = false)
        {
            _colorTween?.Kill();
            _colorTween = _renderer.material.DOColor(targetColor, duration).SetEase(ease);

            if (autoRevert)
            {
                _colorTween.OnComplete(() =>
                {
                    AnimateColor(_defaultColor, duration, ease);
                });
            }
        }
        
        private void OnDisable()
        {
            _scaleTween?.Kill();
            _colorTween?.Kill();
        }
    }
}