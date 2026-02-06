using Game.Scripts.UI.Controls;
using Game.Scripts.UI.Pages;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Utilities.MonoDebugs
{
    public class ButtonDebug : MonoBehaviour
    {
        [SerializeField] private MenuButton _buttonToDisable;

        [SerializeField] private InputActionReference _referenceA;
        [SerializeField] private InputActionReference _referenceB;
        
        public void HelloConsole()
        {
            Debug.Log("Hello Console");
        }
        
        public void PageTypeDebug(PageType pageType)
        {
            Debug.Log($"Page type call - {pageType}");
        }

        private void Update()
        {
            if (_referenceA.action.WasPerformedThisFrame())
            {
                _buttonToDisable?.SetActiveButton(false);
            }

            if (_referenceB.action.WasPerformedThisFrame())
            {
                _buttonToDisable?.SetActiveButton(true);
            }
        }
    }
}