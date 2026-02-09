using UnityEngine;

namespace Game.Scripts.Core.Audio.SfxPlayers
{
    public class LeoSfx : MonoBehaviour
    {
        public void Bark()
        {
            G.EventBus.Publish(SoundType.DogBark);
        }
        
        public void Whine()
        {
            G.EventBus.Publish(SoundType.DogWhine);
        }
        
        public void Step()
        {
            G.EventBus.Publish(SoundType.DogBark);
        }
    }
}