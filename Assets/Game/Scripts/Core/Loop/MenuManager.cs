using System.Collections.Generic;
using Game.Scripts.Setups;
using Game.Scripts.UI.Screens.Menu.Pages;
using Game.Scripts.Utilities.Effects;
using Game.Scripts.Utilities.Events;
using UnityEngine;

namespace Game.Scripts.Core.Loop
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Space] private GameObject _mainMenuBackground;
        [SerializeField] private CanvasGroupFader _menuFader;
        
        [SerializeField, Space] private List<EnumItem<PageType, Page>> _menuPages;
        
        [SerializeField, Space] private PageType _startPageType = PageType.Main;
        [SerializeField] private PageType _overlayMenuPageType = PageType.Overlay;
        
        private readonly Dictionary<PageType, Page> _mappedPages = new Dictionary<PageType, Page>();
        
        private Page _previousPage;
        private Page _currentPage;

        private bool _isInitialized;
        
        public void Initialize()
        {
            foreach (var page in _menuPages)
            {
                if (_mappedPages.ContainsKey(page.Key))
                {
                    Debug.LogWarning("[MenuManager.Initialize] Error while trying to map dictionary, menu is not initialized.");
                    return;
                }
                
                _mappedPages.Add(page.Key, page.Value);
                
                page.Value.Initialize();
                page.Value.Hide();
            }

            if (!_mappedPages.TryGetValue(_startPageType, out _currentPage))
            {
                Debug.LogWarning("[MenuManager.Initialize] No start page in Dictionary.");
                return;
            }
            
            _isInitialized = true;
            
            ShowBackground();
            HandlePageTypeChange(_startPageType);
            
            if (G.IsTestMode)
            {
                HideBackground();
                Close();
            }
            
            G.EventBus.Subscribe<PageType>(HandlePageTypeChange);
        }

        public void Toggle()
        {
            if (!_currentPage)
            {
                OpenOverlay();
                return;
            }
            
            Close();
        }
        
        public void Open()
        {
            Close();
            
            HandlePageTypeChange(_startPageType);
            
            _menuFader.FadeIn();
            
            G.Cursor.Unlock();
        }
        
        public void OpenOverlay()
        {
            Close();
            
            HandlePageTypeChange(_overlayMenuPageType);
            
            _menuFader.FadeIn();
            
            G.EventBus.Publish(new OnGamePausedEvent { IsPaused = true });
            G.Cursor.Unlock();
        }
        
        public void Close()
        {
            if (!_currentPage)
                return;
            
            _currentPage.Hide();
            _currentPage.OnClosed.Invoke();
            
            _currentPage = null;
            _previousPage = null;
            
            _menuFader.CutOut();
            
            G.EventBus.Publish(new OnGamePausedEvent { IsPaused = false });
            G.Cursor.Lock();
        }

        public void ToPreviousPage()
        {
            if (!_previousPage)
            {
                Debug.Log("[MenuManager.ToPreviousPage] No cached previous page.");
                return;
            }
            
            HandlePageTypeChange(_previousPage.Type);
            _previousPage = null;
        }

        public void ShowBackground()
        {
            _mainMenuBackground.SetActive(true);
        }
        
        public void HideBackground()
        {
            _mainMenuBackground.SetActive(false);
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

            if (_currentPage)
            {
                _currentPage.Hide();
                _currentPage.OnClosed.Invoke();
                
                _previousPage = _currentPage;
            }
            
            _currentPage = newPage;
            _currentPage.Show();
        }
    }
}