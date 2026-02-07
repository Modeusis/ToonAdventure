using Game.Scripts.Core.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Scripts.Core.Levels
{
    public abstract class Level : MonoBehaviour
    {
        [field: SerializeField, Space] public LevelType Type { get; private set; }
        [field: SerializeField, Space] public MusicType MusicType { get; private set; }
        
        [field: SerializeField, Space] public Transform StartPoint { get; private set; }
        
        public virtual void Load()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Unload()
        {
            gameObject.SetActive(false);
        }
    }
}