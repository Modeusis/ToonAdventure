using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Scripts.Utilities.Load
{
    public class AddressableLoader : IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new ();
        private readonly Dictionary<string, List<GameObject>> _instanceHandles = new ();
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _scenesHandles = new ();

        public async UniTask<T> LoadAsync<T>(string referenceKey) where T : Object
        {
            if (_assetHandles.TryGetValue(referenceKey, out var handle))
            {
                return handle.Result as T;
            }
            
            try
            {
                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    var gameObjectLoadOperation = Addressables.LoadAssetAsync<GameObject>(referenceKey);
                    var gameObject = await gameObjectLoadOperation;
                    
                    if (gameObject == null)
                    {
                        Debug.LogError($"[Addressable] Failed to load {typeof(T).Name} from {referenceKey}");
                        
                        return null;
                    }
                    
                    var component = gameObject.GetComponent<T>();
                    
                    if (component == null)
                    {
                        Debug.LogError($"[Addressable] Failed to get component {typeof(T).Name} from loaded prefab {referenceKey}");
                        
                        return null;
                    }
                
                    if (gameObjectLoadOperation.Status == AsyncOperationStatus.Succeeded && component != null)
                    {
                        _assetHandles[referenceKey] = gameObjectLoadOperation;
                        return component;
                    }
                
                    if (gameObjectLoadOperation.IsValid())
                        Addressables.Release(gameObjectLoadOperation);
                }
                else
                {
                    var loadOperation = Addressables.LoadAssetAsync<T>(referenceKey);
                    var asset = await loadOperation;
                    
                    if (asset == null)
                    {
                        Debug.LogError($"[Addressable] Failed to load {typeof(T).Name} from {referenceKey}");
                        
                        return null;
                    }
                
                    if (loadOperation.Status == AsyncOperationStatus.Succeeded && asset != null)
                    {
                        _assetHandles[referenceKey] = loadOperation;
                        return asset;
                    }
                
                    if (loadOperation.IsValid())
                        Addressables.Release(loadOperation);
                }

                Debug.LogError($"[Addressable] Failed to load {typeof(T).Name} from {referenceKey}");
                
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Addressable] Error loading asset {referenceKey}: {ex.Message}");

                return null;
            }
        }

        public async UniTask<T> InstantiateAsync<T>(string referenceKey, Transform parent = null,
            string instanceName = null) where T : Object
        {
            try
            {
                GameObject instance = await Addressables.InstantiateAsync(referenceKey, parent);

                if (!string.IsNullOrEmpty(instanceName))
                {
                    instance.name = instanceName;
                }   
                    
                if (!_instanceHandles.TryGetValue(referenceKey, out var instanceList))
                {
                    instanceList = new List<GameObject> { instance };
                        
                    _instanceHandles[referenceKey] = instanceList;
                }

                var component = instance.GetComponent<T>();

                if (component == null)
                {
                    Debug.LogError($"[Addressable] Prefab loaded, Failed to get component {typeof(T).Name} from {referenceKey} instance");
                        
                    return null;
                }
                    
                return component;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Addressable] Failed to load {typeof(T).Name} from {referenceKey}: {ex.Message}");
                
                return null;
            }
        }

        public async UniTask<SceneInstance> LoadSceneAsync(string referenceKey,
            LoadSceneMode mode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            if (_scenesHandles.TryGetValue(referenceKey, out var handle))
            {
                if (handle.IsValid())
                {
                    Debug.Log($"[Addressable] Scene {referenceKey} already loaded");
                    return await handle;
                }
                
                _scenesHandles.Remove(referenceKey);
            }
            
            try
            {
                var loadOperation = Addressables.LoadSceneAsync(referenceKey, mode);
                var sceneInstance = await loadOperation;

                if (loadOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    _scenesHandles[referenceKey] = loadOperation;
                    
                    return sceneInstance;
                }
                
                Debug.LogError($"[Addressable] Failed to scene from {referenceKey}");
                
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Addressable] Failed to load {referenceKey}: {e.Message}");
                
                return default;
            }
        }

        public void Dispose()
        {
            foreach (var instanceList in _instanceHandles.Values)
            {
                foreach (var instance in instanceList)
                {
                    if (instance == null)
                        continue;
                    
                    Addressables.ReleaseInstance(instance);
                }
            }
            _instanceHandles.Clear();

            foreach (var operationHandle in _assetHandles.Values)
            {
                Addressables.Release(operationHandle);
            }
            _assetHandles.Clear();

            foreach (var scenesHandle in _scenesHandles.Values)
            {
                Addressables.UnloadSceneAsync(scenesHandle);
            }
            _scenesHandles.Clear();
        }
    }
}