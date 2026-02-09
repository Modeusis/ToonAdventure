using System;
using DG.Tweening;
using Game.Scripts.Core.Audio;
using Game.Scripts.Setups.Animations;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Interactions.Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField, Space] private InteractableObject _interactionZone;
        [SerializeField] private Transform _pickupTransform;
        //TODO добавить эффект подбора
        
        [SerializeField, Space] private Vector3AnimationProperty _hideScaleProperty;

        private Collider _collider;
        
        private Tween _hideTween;
        private bool _isPickedUp;
        
        private void Awake()
        {
            _interactionZone.OnInteractionProceed.AddListener(PickupObject);
            
            _collider = GetComponent<Collider>();
        }

        private void OnDestroy()
        {
            _interactionZone.OnInteractionProceed.RemoveListener(PickupObject);
        }

        private void PickupObject()
        {
            if (_isPickedUp)
                return;
            
            G.Audio.PlaySfx(SoundType.ItemPickUp);

            if (_collider)
            {
                _collider.enabled = false;
            }
            
            _hideTween?.Kill();
            _hideTween = null;
            
            _hideTween = _pickupTransform
                .DOScale(_hideScaleProperty.Target, _hideScaleProperty.Duration)
                .SetEase(_hideScaleProperty.Curve);
            
            _isPickedUp = true;
        }
    }
}