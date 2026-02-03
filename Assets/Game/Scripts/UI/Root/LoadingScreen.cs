using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Root
{
    [RequireComponent(typeof(Canvas))]
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField, Range(5, 100), Space] private int symbolMsDelay = 15;
        
        [SerializeField, Space] private TMP_Text _loadingTextField;

        public bool IsShown => gameObject.activeSelf;
        
        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _loadTextLock = new ();
        
        private string LoadingText
        {
            get => _loadingTextField.text;
            set
            {
                if (_loadingTextField.text == value)
                {
                    return;
                }
                
                _loadingTextField.text = value;
            }
        }
        
        public void HideLoadingScreen()
        {
            CancelTextChange();
            gameObject.SetActive(false);
        }
        
        public void ShowLoadingScreen(string loadingText = null)
        {
            gameObject.SetActive(true);
            
            if (string.IsNullOrEmpty(loadingText))
                return;
            
            ChangeLoadingScreenText(loadingText).Forget();
        }
        
        private async UniTask ChangeLoadingScreenText(string text)
        {
            if (_loadingTextField.text == text)
            {
                return;
            }

            lock (_loadTextLock)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }
            
            try
            {
                for (int j = 0; j < _loadingTextField.text.Length; j++)
                {
                    LoadingText = LoadingText.Substring(0, LoadingText.Length - 1);
                
                    await UniTask.Delay(TimeSpan.FromMilliseconds(symbolMsDelay), cancellationToken: _cancellationTokenSource.Token);
                }
            
                LoadingText = "";
            
                for (int i = 0; i < text.Length; i++)
                {
                    LoadingText += text[i];

                    await UniTask.Delay(TimeSpan.FromMilliseconds(symbolMsDelay), cancellationToken: _cancellationTokenSource.Token);
                }
            
                LoadingText = text;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[LoadingScreen.ChangeLoadingScreenText] Cancelling loading screen text change");
            }
        }

        private void CancelTextChange()
        {
            lock (_loadTextLock)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}