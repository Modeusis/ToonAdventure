using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Utilities.Events
{
    public struct OnPuzzleSolvedEvent { }

    public struct OnPuzzleBeginEvent
    {
        public Transform PlayerTransform;
    }
    
    public struct OnPuzzleStopEvent { }
}