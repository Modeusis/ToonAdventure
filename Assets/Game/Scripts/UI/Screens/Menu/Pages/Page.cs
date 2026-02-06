using Game.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.UI.Screens.Menu.Pages
{
    public abstract class Page : MonoBehaviour
    {
        [field: SerializeField, Space] public PageType Type { get; private set;}

        public UnityEvent OnClosed { get; } = new UnityEvent();

        public abstract void Initialize();
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        protected virtual void ToPage(PageType newPageType)
        {
            Debug.Log($"ToPage: {newPageType}");
            
            G.EventBus.Publish(newPageType);
        }
    }
}