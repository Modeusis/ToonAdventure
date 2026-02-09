using DG.Tweening;
using Game.Scripts.Setups.Animations;
using Game.Scripts.Utilities.Effects;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Screens.HUD
{
    public class DynamicTooltip : MonoBehaviour
    {
        [SerializeField, Space] private CanvasGroupFader _fader;
        [SerializeField] private TMP_Text _text;

        [SerializeField, Space] private Vector3AnimationProperty _showScaleProperty;
        [SerializeField] private Vector3AnimationProperty _hideScaleProperty;
        
        [SerializeField] private Transform _tooltipTransform;
        
        private bool _isShowing;
        
        private Tween _showTween;
        
        public void Show(string text)
        {
            _text.text = text;
            
            _showTween?.Kill();
            _showTween = null;
            
            _showTween = _tooltipTransform
                .DOScale(_showScaleProperty.Value, _showScaleProperty.Duration)
                .SetEase(_showScaleProperty.Ease);
            
            _fader.FadeIn();
            
            _isShowing = true;
        }

        public void Hide()
        {
            if (!_isShowing)
                return;
            
            _fader.FadeOut();
            
            _showTween?.Kill();
            _showTween = null;
            
            _showTween = _tooltipTransform
                .DOScale(_hideScaleProperty.Value, _hideScaleProperty.Duration)
                .SetEase(_hideScaleProperty.Ease);
            
            _isShowing = false;
        }

        public void HideFast()
        {
            _showTween?.Kill();
            _fader.CutOut();
            
            _isShowing = false;
        }
    }
}