using System;
using System.Collections;
using Game.Scripts.Utilities.Pool;
using UnityEngine;

namespace Game.Scripts.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IPoolable
    {
        private AudioSource _audioSource;
        
        private Coroutine _waitCoroutine;
        private Coroutine _followCoroutine; // Корутина для следования

        public event Action<AudioPlayer> OnReleased;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void OnDisable()
        {
            StopFollow();
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
        
        public void Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool isLooped = false, float spatialBlend = 0f, Transform followTarget = null)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            _audioSource.loop = isLooped;
            _audioSource.spatialBlend = spatialBlend;
            
            if (spatialBlend > 0)
            {
                _audioSource.rolloffMode = AudioRolloffMode.Linear;
                _audioSource.minDistance = 1f;
                _audioSource.maxDistance = 20f;
                _audioSource.dopplerLevel = 0f;
            }
            
            if (followTarget != null)
            {
                transform.position = followTarget.position;
                _followCoroutine = StartCoroutine(FollowTargetRoutine(followTarget));
            }
            else
            {
                StopFollow();
            }

            _audioSource.Play();
            
            if (!isLooped)
            {
                _waitCoroutine = StartCoroutine(WaitingCoroutine(clip.length));
            }
        }

        public void StopSound()
        {
            OnReleased?.Invoke(this);
        }
        
        public void Release()
        {
            _audioSource.Stop();
            StopFollow();

            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                _waitCoroutine = null;
            }
            
            _audioSource.transform.localPosition = Vector3.zero;
            _audioSource.clip = null;
            _audioSource.loop = false;
        }

        private void StopFollow()
        {
            if (_followCoroutine != null)
            {
                StopCoroutine(_followCoroutine);
                _followCoroutine = null;
            }
        }
        
        private IEnumerator FollowTargetRoutine(Transform target)
        {
            while (target)
            {
                transform.position = target.position;
                yield return null;
            }
            
            _followCoroutine = null;
        }

        private IEnumerator WaitingCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            OnReleased?.Invoke(this);
        }
    }
}