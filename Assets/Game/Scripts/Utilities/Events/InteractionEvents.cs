using Game.Scripts.Core.Interactions;
using UnityEngine;

namespace Game.Scripts.Utilities.Events
{
    public struct OnInteractionZoneEnterEvent
    {
        public IInteractable Interactable;
        public Transform Transform;
    }

    public struct OnInteractionZoneExitEvent
    {
        public IInteractable Interactable;
    }
}