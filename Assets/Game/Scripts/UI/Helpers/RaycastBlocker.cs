using Game.Scripts.Utilities.Effects;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.UI.Helpers
{
    public class RaycastBlocker : MonoBehaviour
    {
        [SerializeField] private CanvasGroupFader _fader;

        private bool _isInitialized;
        
        public void Initialize()
        {
            if (_isInitialized)
                return;
            
            _fader.IsBlockRaycast = false;
            _fader.CutOut();
            
            _isInitialized = true;
        }
        
        public void HandleBlockerStateChange(OnPopUpRaycastBlockerEvent eventData)
        {
            if (!eventData.IsBlocking)
            {
                Hide();
                return;
            }

            Show();
        }
        
        private void Hide()
        {
            _fader.IsBlockRaycast = false;
            _fader.FadeOut();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            
            _fader.IsBlockRaycast = true;
            _fader.FadeIn();
        }
    }
}