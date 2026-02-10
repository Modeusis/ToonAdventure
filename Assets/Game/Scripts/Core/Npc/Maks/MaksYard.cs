using Game.Scripts.Core.Interactions;
using UnityEngine;

namespace Game.Scripts.Core.NPC.Maks
{
    public class MaksYard : StateSequenceInteractable<MaksState>
    {
        [Header("Components")]
        [field: SerializeField] public NpcNavigator Navigator { get; private set; }
        [field: SerializeField] public NpcAnimationController AnimationController { get; private set; }
    }
}