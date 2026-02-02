using Game.Scripts.UI.Pages;
using UnityEngine;

namespace Game.Scripts.Setups.UI
{
    [CreateAssetMenu(fileName = "New menu pages setup", menuName = "Setup/Menu pages")]
    public class MenuPagesSetup : EnumDictionarySetup<PageType, Page>
    {
        
    }
}