using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI.Controls
{
    public class MenuSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _valueText;

        [Header("Settings")]
        [Tooltip("Формат отображения текста. {0:0} - целое число, {0:0.0} - один знак после запятой")]
        [SerializeField] private string _textFormat = "{0:0}";
        
        [Tooltip("Если true, значение 0.5 будет отображаться как 50")]
        [SerializeField] private bool _multiplyBy100ForText = false;
        
        public UnityEvent<float> OnValueChanged = new UnityEvent<float>();
        
        public float CurrentValue => _slider.value;

        private void Awake()
        {
            if (_slider == null)
                _slider = GetComponent<Slider>();
            
            _slider.onValueChanged.AddListener(HandleSliderChanged);
            
            UpdateText(_slider.value);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(HandleSliderChanged);
            OnValueChanged.RemoveAllListeners();
        }

        private void HandleSliderChanged(float value)
        {
            UpdateText(value);
            
            OnValueChanged?.Invoke(value);
        }

        private void UpdateText(float value)
        {
            if (_valueText == null) 
                return;

            float displayValue = _multiplyBy100ForText ? value * 100f : value;
            _valueText.text = string.Format(_textFormat, displayValue);
        }
        
        public float GetValue()
        {
            return _slider.value;
        }
        
        public void SetValue(float value, bool notifyListeners = true)
        {
            if (notifyListeners)
            {
                _slider.value = value;
            }
            else
            {
                _slider.SetValueWithoutNotify(value);
                UpdateText(value);
            }
        }
    }
}