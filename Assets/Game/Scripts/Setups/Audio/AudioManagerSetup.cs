using System;
using Game.Scripts.Core.Audio;
using UnityEngine;

namespace Game.Scripts.Setups.Audio
{
    [CreateAssetMenu(fileName = "New audio service setup", menuName = "Setup/Audio service")]
    public class AudioServiceSetup : ScriptableObject
    {
        [Header("Sound settings")]
        [SerializeField, Range(0f, 1f), Space] private float _volumeSfx = 0.5f; 
        [SerializeField, Range(0f, 1f)] private float _volumeMusic = 0.5f; 
        
        [SerializeField, Space] private Vector2 _sfxPitchRange = new Vector2(0.95f, 1.05f);
        
        [Tooltip("X value = FadeIn duration, Y value = FadeOut duration, Z value = StayIn position")]
        [SerializeField] private Vector3 _fadeInOutRange = new Vector3(0.3f, 0.3f, 0.2f);
        
        [Header("Actual sound clips")]
        [SerializeField, Space] private EffectProperty[] _sfxProperties;
        [SerializeField] private MusicProperty[] _musicProperties;
        
        public float VolumeSfx => _volumeSfx;
        public float VolumeMusic => _volumeMusic;
        
        public float MusicFadeInDuration => _fadeInOutRange.x;
        public float MusicFadeOutDuration => _fadeInOutRange.y;
        public float FadeStayIn => _fadeInOutRange.z;
        
        public Vector2 PitchRange => _sfxPitchRange;
        
        public EffectProperty[] Sfx => _sfxProperties;
        public MusicProperty[] Music => _musicProperties;
    }

    [Serializable]
    public class EffectProperty
    {
        [SerializeField] private SoundType _audioType;
        [SerializeField] private AudioClip[] _audioClips;
        
        public SoundType Type => _audioType;
        public AudioClip[] Clips => _audioClips;
    }
    
    [Serializable]
    public class MusicProperty
    {
        [SerializeField] private MusicType _audioType;
        [SerializeField] private AudioClip[] _audioClips;
        
        public MusicType Type => _audioType;
        public AudioClip[] Clips => _audioClips;
    }
}