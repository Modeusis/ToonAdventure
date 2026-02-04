using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Utilities.MonoDebugs
{
    public class InputDebugger : MonoBehaviour
    {
        private void Update()
        {
            if (!G.IsReady)
                return;

            if (G.Input.Game.Back.WasPerformedThisFrame())
            {
                G.UI.Screens.Menu.Toggle();
            }
        }
    }
}