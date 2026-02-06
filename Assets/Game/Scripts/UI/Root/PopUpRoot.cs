using Game.Scripts.Core;
using Game.Scripts.UI.Helpers;
using Game.Scripts.UI.PopUps;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    [RequireComponent(typeof(Canvas))]
    public class PopUpRoot : MonoBehaviour
    {
        [SerializeField, Space] private RaycastBlocker _raycastBlocker;
        [field: SerializeField, Space] public ConfirmationPopUp Confirmation { get; private set; } 

        public void Initialize()
        {
            Confirmation.Initialize();
            
            _raycastBlocker.Initialize();
            G.EventBus.Subscribe<OnPopUpRaycastBlockerEvent>(_raycastBlocker.HandleBlockerStateChange);
        }
    }
}