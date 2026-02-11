using System;
using Game.Scripts.Core.Audio;
using Game.Scripts.Core.Cutscene.CutsceneStates;
using Unity.Cinemachine;

namespace Game.Scripts.Core.Cutscene
{
    [Serializable]
    public struct CutsceneConfig
    {
        public CutsceneState State;
        public CinemachineCamera VirtualCamera;
        public SoundType Sound;
        public float Duration;
    }
}