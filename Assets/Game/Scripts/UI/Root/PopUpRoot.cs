using Game.Scripts.UI.PopUps;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    [RequireComponent(typeof(Canvas))]
    public class PopUpRoot : MonoBehaviour
    {
        [field: SerializeField, Space] public ConfirmationPopUp Confirmation { get; private set; } 
        
        private Canvas _canvas;

        public void Initialize()
        {
            _canvas = GetComponent<Canvas>();
        }
    }
}