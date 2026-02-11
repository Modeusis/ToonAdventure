using System;
using DG.Tweening;
using Game.Scripts.Core.Audio;
using Game.Scripts.Setups.Animations;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.Core.Interactions.Pickups
{
    public class Pickup : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform _pickupTransform;
        
        [SerializeField, Space] private Vector3AnimationProperty _hideScaleProperty;
        
        [Header("Effects")]
        [SerializeField, Space] private ParticleSystem _pickupEffect;
        [SerializeField] private bool _isPlayingPickupEffect = true;
        
        [field: SerializeField, Space] public UnityEvent OnPickup { get; private set;  } = new UnityEvent();

        private Collider _collider;
        
        private Tween _hideTween;
        private bool _isPickedUp;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _pickupEffect.Stop();
        }

        private void OnDestroy()
        {
            OnPickup?.RemoveAllListeners();
        }

        public void PickupObject()
        {
            if (_isPickedUp)
                return;
            
            G.Audio.PlaySfx(SoundType.ItemPickUp);
            
            if (_isPlayingPickupEffect)
            {
                _pickupEffect.Play();
            }
            
            if (_collider)
            {
                _collider.enabled = false;
            }
            
            _hideTween?.Kill();
            _hideTween = null;
            
            _hideTween = _pickupTransform
                .DOScale(_hideScaleProperty.Value, _hideScaleProperty.Duration)
                .SetEase(_hideScaleProperty.Ease);
            
            OnPickup?.Invoke();
            
            _isPickedUp = true;
        }
    }
}