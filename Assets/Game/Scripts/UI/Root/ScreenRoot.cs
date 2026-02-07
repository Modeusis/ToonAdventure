using Game.Scripts.UI.Screens.Dialog;
using Game.Scripts.UI.Screens.Menu;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenRoot : MonoBehaviour
    {
        [field: SerializeField, Space] public MenuManager Menu { get; private set; }
        [field: SerializeField] public DialogueView Dialogue { get; private set; }
        //Add player hud
        //Add static ui
        //Add Quest and tooltips
        
        public void Initialize()
        {
            Menu.Initialize();
        }
    }
}