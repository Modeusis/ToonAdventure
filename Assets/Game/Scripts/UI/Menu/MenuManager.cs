using System;
using System.Collections.Generic;
using Game.Scripts.Setups.UI;
using Game.Scripts.UI.Pages;
using UnityEngine;

namespace Game.Scripts.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MenuPagesSetup _menuPagesSetup;
        
        private Dictionary<PageType, Page> _mappedPages;
        
        private Page _currentPage;

        private bool _isInitialized;
        
        private void Awake()
        {
            if (!_menuPagesSetup.TryMapDictionary(out _mappedPages))
            {
                Debug.LogWarning("[MenuManager.Awake] Error while trying to map dictionary, menu is not initialized.");
                return;
            }
            
            _isInitialized = true;
        }

        public void HandlePageTypeChange(PageType newPageType)
        {
            if (!_isInitialized)
                return;
            
            if (_currentPage.Type == newPageType)
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