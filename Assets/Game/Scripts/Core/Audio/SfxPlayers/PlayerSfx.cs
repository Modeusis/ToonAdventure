using UnityEngine;

namespace Game.Scripts.Core.Audio.SfxPlayers
{
    public class PlayerSfx : MonoBehaviour
    {
        private SoundType _stepSoundType = SoundType.StepWood;
        
        public void SetFootstepType(SoundType stepSoundType) => _stepSoundType = stepSoundType;
        
        public void Footstep()
        {
            G.Audio.PlaySfx(_stepSoundType);
        }
    }
}