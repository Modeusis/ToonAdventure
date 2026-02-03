using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Setups;
using Game.Scripts.UI.Pages;
using Game.Scripts.Utilities.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private List<EnumItem<PageType, Page>> _menuPages;
        [SerializeField, Space] private PageType _startPageType = PageType.Main;
        
        private readonly Dictionary<PageType, Page> _mappedPages = new Dictionary<PageType, Page>();
        
        private Page _currentPage;

        private bool _isInitialized;
        
        public void Initialize()
        {
            foreach (var page in _menuPages)
            {
                if (_mappedPages.ContainsKey(page.Key))
                {
                    Debug.LogWarning("[MenuRoot.Awake] Error while trying to map dictionary, menu is not initialized.");
                    return;
                }
                
                _mappedPages.Add(page.Key, page.Value);
                
                page.Value.Initialize();
                page.Value.Hide();
            }

            if (!_mappedPages.TryGetValue(_startPageType, out _currentPage))
            {
                Debug.LogWarning("No start page in Dictionary.");
                return;
            }
            
            _isInitialized = true;
            
            HandlePageTypeChange(_startPageType);
            
            G.EventBus.Subscribe<PageType>(HandlePageTypeChange);
        }
        
        private void HandlePageTypeChange(PageType newPageType)
        {
            if (!_isInitialized)
                return;

            if (!_mappedPages.TryGetValue(newPageType, out var newPage))
            {
                Debug.LogWarning($"[MenuManager.HandlePageTypeChange] Detect not mapped page type: {newPageType}");
                return;
            }
            
            _currentPage?.Hide();
            
            _currentPage = newPage;
            _currentPage.Show();
        }
    }
}