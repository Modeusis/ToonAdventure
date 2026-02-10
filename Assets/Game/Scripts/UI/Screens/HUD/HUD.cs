using Game.Scripts.Core;
using Game.Scripts.Utilities.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.UI.Screens.HUD
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public CanvasGroupFader StaticTooltip { get; private set; }
        [field: SerializeField] public DynamicTooltip DynamicTooltip { get; private set; }
        
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            if (!G.IsTestMode)
            {
                Hide();
            }
            
            _isInitialized = true;
        }

        public void Hide()
        {
            StaticTooltip.CutOut();
            DynamicTooltip.HideFast();
        }
    }
}