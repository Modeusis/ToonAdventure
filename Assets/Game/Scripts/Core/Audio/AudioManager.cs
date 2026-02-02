using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Setups;
using Game.Scripts.Setups.Audio;
using Game.Scripts.Utilities.Extensions;
using Game.Scripts.Utilities.Pool;
using UnityEngine;

namespace Game.Scripts.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("SFX Player Pool")]
        [SerializeField, Space] private AudioPlayer _sfxPlayerPrefab;
        [SerializeField] private Transform _playersContainer;
        [SerializeField, Min(1f)] private int _minPoolSize = 5;
        [SerializeField, Min(1f)] private int _maxPoolSize = 20;
        
        [Header("Music player")]
        [SerializeField, Space] private AudioSource _musicPlayer;
        
        [SerializeField, Space] private bool _isMusicPlayingOnStart;
        [SerializeField] private MusicType _backgroundMusicOnStart = MusicType.BackgroundMusic;
        
        [SerializeField, Space] private AudioServiceSetup _setup;

        private bool _isMusicCurrentlyPlaying;
        private Tween _musicFade;
        
        private Coroutine _musicFadeCoroutine;
        private WaitForSeconds _musicStayInWaiter;
        
        private AbstractPool<AudioPlayer> _sfxPlayerPool;
        
        public void Awake()
        {
            _musicPlayer?.Stop();
            
            _musicStayInWaiter = new WaitForSeconds(_setup.FadeStayIn);
        }

        public void Start()
        {
            InitializePool();

            if (!_isMusicPlayingOnStart || _isMusicCurrentlyPlaying)
                return;
            
            PlayMusic(_backgroundMusicOnStart);
        }
        
        public void PlaySfx(SoundType soundType)
        {
            var effectProperty = _setup.Sfx.FirstOrDefault(sfx => sfx.Type == soundType);

            if (effectProperty == null)
            {
                Debug.LogWarning($"[SimpleAudioService.PlaySfx] Property with sound type {soundType} not found");
                return;
            }

            var sfxPlayer = _sfxPlayerPool.Get();
            
            var pitch = _setup.PitchRange.GetRandomInRange();
            var volume = _setup.VolumeSfx;
            
            sfxPlayer.OnReleased += ReleaseSfxPlayer;
            sfxPlayer.Play(effectProperty.Clips.GetRandom(), volume, pitch);
        }

        public void PlayMusic(MusicType musicType)
        {
            var musicProperty = _setup.Music.FirstOrDefault(music => music.Type == musicType);

            if (musicProperty == null)
            {
                Debug.LogWarning($"[SimpleAudioService.PlayMusic] Property with music type {musicType} not found");
                return;
            }

            var clips = musicProperty.Clips;
            if (clips.Length <= 0)
            {
                Debug.LogWarning($"[SimpleAudioService.PlayMusic] No clips found in setup for {musicType}");
                return;
            }
            
            var musicClip = clips.Length > 1 ? clips.GetRandom() : clips[0];

            if (_musicFadeCoroutine != null)
            {
                StopCoroutine(_musicFadeCoroutine);
                _musicFadeCoroutine = null;
            }
            
            _musicFadeCoroutine = StartCoroutine(PlayMusicSequence(musicClip));
        }

        private void InitializePool()
        {
            if (_sfxPlayerPool != null)
                return;

            _sfxPlayerPool = new AbstractPool<AudioPlayer>(_sfxPlayerPrefab, _playersContainer, true, _minPoolSize, _maxPoolSize);
        }
        
        private IEnumerator PlayMusicSequence(AudioClip musicClip)
        {
            float targetVolume = _setup.VolumeMusic;

            if (_isMusicCurrentlyPlaying)
            {
                FadeMusic(0f, _setup.MusicFadeOutDuration, onComplete: () =>
                {
                    _musicPlayer.Stop();
                    _isMusicCurrentlyPlaying = false;
                });
                yield return _musicFade;
            }
            
            _musicPlayer.volume = 0f;
            _musicPlayer.loop = true;
            _musicPlayer.clip = musicClip;

            yield return _musicStayInWaiter;
            
            _musicPlayer.Play();
            FadeMusic(targetVolume, _setup.MusicFadeInDuration, onStart: () =>
            {
                _isMusicCurrentlyPlaying = true;
            });
        }

        private void FadeMusic(float targetVolume, float duration, Action onStart = null, Action onComplete = null)
        {
            if (!_musicPlayer)
            {
                Debug.LogWarning($"[SimpleAudioService.FadeMusic] Music player is not set");
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