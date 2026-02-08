using UnityEngine;

namespace Game.Scripts.UI.Root
{
    public class UiRoot : MonoBehaviour
    {
        [field: SerializeField, Space] public ScreenRoot Screens { get; private set; }
        [field: SerializeField] public PopUpRoot PopUp { get; private set; }
        [field: SerializeField] public LoadingScreen Loading { get; private set; }
        
        public void Initialize()
        {
            Screens.Initialize();
            PopUp.Initialize();
            
            Loading.Hide();
        }
    }
}