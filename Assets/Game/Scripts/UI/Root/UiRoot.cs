using Game.Scripts.Core;
using Game.Scripts.UI.Menu;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    public class UiRoot : MonoBehaviour
    {
        [field: SerializeField] public ScreenRoot Screens { get; private set; }
        [field: SerializeField, Space] public PopUpRoot PopUp { get; private set; }
        [field: SerializeField] public LoadingScreen Loading { get; private set; }
        
        public void Initialize()
        {
            Screens.Initialize();
            PopUp.Initialize();
            
            Loading.HideLoadingScreen();
        }
    }
}