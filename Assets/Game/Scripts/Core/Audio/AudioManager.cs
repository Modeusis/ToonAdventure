using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Setups.Audio;
using Game.Scripts.Utilities.Events;
using Game.Scripts.Utilities.Extensions;
using Game.Scripts.Utilities.Pool;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Mixer groups")]
        [SerializeField, Space] private AudioMixerGroup _sfxMixerGroup;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        
        [SerializeField, Space] private string _sfxTargetProperty = "SfxVolume";
        [SerializeField] private string _musicTargetProperty = "MusicVolume";
        
        [Header("SFX Player Pool")]
        [SerializeField, Space] private AudioPlayer _sfxPlayerPrefab;
        [SerializeField] private Transform _playersContainer;
        [SerializeField, Min(1f)] private int _minPoolSize = 5;
        [SerializeField, Min(1f)] private int _maxPoolSize = 20;
        
        [Header("Music player")]
        [SerializeField, Space] private AudioSource _musicPlayer;
        
        [SerializeField, Space] private bool _isMusicPlayingOnStart;
        [SerializeField] private MusicType _backgroundMusicOnStart = MusicType.Menu;
        
        [SerializeField, Space] private AudioServiceSetup _setup;

        private bool _isMusicCurrentlyPlaying;
        private Tween _musicFade;
        
        private Coroutine _musicPlaylistCoroutine;
        private WaitForSeconds _musicStayInWaiter;
        
        private AbstractPool<AudioPlayer> _sfxPlayerPool;
        
        private Dictionary<MusicType, AudioRandomContainer> _musicContainers;
        
        public void Awake()
        {
            _musicPlayer?.Stop();
            
            _musicStayInWaiter = new WaitForSeconds(_setup.FadeStayIn);
        }

        public void Initialize()
        {
            InitializePool();

            G.Save.OnMusicVolumeChanged.AddListener(SetMusicVolume);
            G.Save.OnSfxVolumeChanged.AddListener(SetSfxVolume);
            
            _musicContainers = new Dictionary<MusicType, AudioRandomContainer>();
            foreach (var property in _setup.Music)
            {
                _musicContainers[property.Type] = new AudioRandomContainer(property.Clips);
            }
            
            if (!_isMusicPlayingOnStart || _isMusicCurrentlyPlaying)
                return;
            
            if (G.IsReady)
            {
                PlayMusic(_backgroundMusicOnStart);
                return;
            }
            
            G.EventBus.Subscribe<OnGameReadyEvent>(_ =>
            {
                PlayMusic(_backgroundMusicOnStart);
            });
        }
        
        public void SetMusicVolume(float value)
        {
            SetMixerVolume(_musicMixerGroup.audioMixer, _musicTargetProperty, value);
        }

        public void SetSfxVolume(float value)
        {
            SetMixerVolume(_sfxMixerGroup.audioMixer, _sfxTargetProperty, value);
        }

        private void SetMixerVolume(AudioMixer mixer, string parameterName, float volume)
        {
            var dbVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            
            if (volume <= 0.001f)
            {
                dbVolume = -80f;
            }
            
            mixer.SetFloat(parameterName, dbVolume);
        }
        
        public void PlaySfx(SoundType soundType)
        {
            PlaySfxInternal(soundType, null);
        }
        
        public void PlaySfx(SoundType soundType, Transform target)
        {
            PlaySfxInternal(soundType, target);
        }
        
        private void PlaySfxInternal(SoundType soundType, Transform target)
        {
            var effectProperty = _setup.Sfx.FirstOrDefault(sfx => sfx.Type == soundType);

            if (effectProperty == null)
            {
                Debug.LogWarning($"[AudioManager] Property with sound type {soundType} not found");
                return;
            }

            var sfxPlayer = _sfxPlayerPool.Get();
            
            var pitch = _setup.PitchRange.GetRandomInRange();
            var volume = _setup.VolumeSfx;
            
            sfxPlayer.OnReleased += ReleaseSfxPlayer;
            
            var spatialBlend = target ? 1.0f : 0.0f;

            sfxPlayer.Play(effectProperty.Clips.GetRandom(), volume, pitch, false, spatialBlend, target);
        }

        public void PlayMusic(MusicType musicType)
        {
            if (!_musicContainers.ContainsKey(musicType))
            {
                Debug.LogWarning($"[AudioManager] No music container found for type {musicType}");
                return;
            }
            
            if (_musicPlaylistCoroutine != null)
            {
                StopCoroutine(_musicPlaylistCoroutine);
                _musicPlaylistCoroutine = null;
            }
            
            _musicPlaylistCoroutine = StartCoroutine(MusicPlaylistRoutine(musicType));
        }

        private void InitializePool()
        {
            if (_sfxPlayerPool != null)
                return;

            _sfxPlayerPool = new AbstractPool<AudioPlayer>(_sfxPlayerPrefab, _playersContainer, true, _minPoolSize, _maxPoolSize);
        }
        
        private IEnumerator MusicPlaylistRoutine(MusicType musicType)
        {
            if (_isMusicCurrentlyPlaying && _musicPlayer.isPlaying)
            {
                var fadeOutComplete = false;
                FadeMusic(0f, _setup.MusicFadeOutDuration, onComplete: () => fadeOutComplete = true);
                
                yield return new WaitUntil(() => fadeOutComplete);
                
                _musicPlayer.Stop();
            }
            
            _isMusicCurrentlyPlaying = true;
            var container = _musicContainers[musicType];
            
            while (_isMusicCurrentlyPlaying)
            {
                AudioClip nextClip = container.GetNextClip();
                
                if (nextClip == null)
                {
                    Debug.LogWarning($"[AudioManager] No clips in container for {musicType}");
                    yield break;
                }

                _musicPlayer.clip = nextClip;
                _musicPlayer.volume = 0f;
                _musicPlayer.loop = false;
                _musicPlayer.Play();
                
                FadeMusic(_setup.VolumeMusic, _setup.MusicFadeInDuration);
                
                var waitDuration = nextClip.length - _setup.MusicFadeOutDuration;
                
                if (waitDuration > 0)
                {
                    yield return new WaitForSeconds(waitDuration);
                }
                else
                {
                    yield return new WaitForSeconds(nextClip.length);
                }
                
                var fadeOutFinished = false;
                FadeMusic(0f, _setup.MusicFadeOutDuration, onComplete: () => fadeOutFinished = true);
                
                yield return new WaitUntil(() => fadeOutFinished);
            }
        }

        private void FadeMusic(float targetVolume, float duration, Action onStart = null, Action onComplete = null)
        {
            if (!_musicPlayer)
            {
                Debug.LogWarning($"[AudioManager.FadeMusic] Music player is not set");
                return;
            }
            
            _musicFade?.Kill();
            _musicFade = null;
            
            _musicFade = _musicPlayer.DOFade(targetVolume, duration)
                .OnStart(() =>
                {
                    onStart?.Invoke();
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }

        private void ReleaseSfxPlayer(AudioPlayer player)
        {
            player.Release(); 
            player.OnReleased -= ReleaseSfxPlayer;
    
            _sfxPlayerPool?.Release(player);
        }
    }
}