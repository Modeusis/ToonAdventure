using Game.Scripts.Core.Loop;
using Game.Scripts.UI.Screens.Dialog;
using Game.Scripts.UI.Screens.HUD;
using Game.Scripts.UI.Screens.Menu;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenRoot : MonoBehaviour
    {
        [field: SerializeField, Space] public MenuManager Menu { get; private set; }
        [field: SerializeField] public HUD HUD { get; private set; }
        
        public void Initialize()
        {
            Menu.Initialize();
            HUD.Initialize();
        }
    }
}