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

        private YieldInstruction _waitDuration;
        
        public event Action<AudioPlayer> OnReleased;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }
        
        public void Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool isLooped = false)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            _audioSource.loop = isLooped;
            
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

            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                
                _waitCoroutine = null;
            }
            
            _audioSource.transform.localPosition = Vector3.zero;
            
            _audioSource.clip = null;
            _audioSource.loop = false;
        }

        private IEnumerator WaitingCoroutine(float duration)
        {
            _waitDuration ??= new WaitForSeconds(duration);
            
            yield return _waitDuration;
            
            OnReleased?.Invoke(this);
        }
    }
}