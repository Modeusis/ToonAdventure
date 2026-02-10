using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Core.Audio;
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
        [SerializeField] private Vector3AnimationProperty _hoverScale;
        [SerializeField] private Vector3AnimationProperty _clickPunchScale;
        [SerializeField] private ColorAnimationProperty _hoverColor;
        [SerializeField] private ColorAnimationProperty _pressColor;

        [Header("Press Movement Settings")]
        [SerializeField] private Vector3 _pressOffset = new Vector3(0, -0.05f, 0); 
        [SerializeField] private float _pressMoveDuration = 0.1f;
        [SerializeField] private AnimationCurve _pressCurve;

        private Color _defaultColor;
        private Vector3 _defaultScale;
        private Vector3 _defaultLocalPosition;
        
        private Tween _colorTween;
        private Tween _scaleTween;
        private Tween _moveTween;

        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            
            _defaultColor = _renderer.material.color;
            _defaultScale = transform.localScale;
            _defaultLocalPosition = transform.localPosition;
        }

        public void OnBeginHover()
        {
            AnimateScale(_hoverScale.Value, _hoverScale.Duration, _hoverScale.Ease);
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
            transform.DOPunchScale(_clickPunchScale.Value, _clickPunchScale.Duration, 10, 1);

            AnimateColor(_pressColor.Value, _pressColor.Duration, _pressColor.Ease, true);
            
            AnimatePressMovement();

            _puzzleController.OnButtonInput(_id);
        }

        private void AnimatePressMovement()
        {
            _moveTween?.Kill(true);

            var pressSequence = DOTween.Sequence();

            pressSequence.Append(transform.DOLocalMove(_defaultLocalPosition + _pressOffset, _pressMoveDuration * 0.5f)
                .SetEase(_pressCurve));

            G.Audio.PlaySfx(SoundType.PuzzleButtonPress);
            
            pressSequence.Append(transform.DOLocalMove(_defaultLocalPosition, _pressMoveDuration * 0.5f)
                .SetEase(Ease.OutElastic));

            _moveTween = pressSequence;
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
            _moveTween?.Kill();
            
            transform.localPosition = _defaultLocalPosition; 
        }
    }
}