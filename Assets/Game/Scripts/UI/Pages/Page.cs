using UnityEngine;

namespace Game.Scripts.UI.Pages
{
    public abstract class Page : MonoBehaviour
    {
        [field: SerializeField] public PageType Type { get; private set;}
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}