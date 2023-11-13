using System;
using System.Collections.Generic;
using System.Linq;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameplayManagers
{
    public class LayoutLoader : MonoBehaviour
    {
        [SerializeField] 
        private LayoutSettings _layoutSettings;

        private AsyncOperationHandle<SceneInstance> _loadHandle;
        private SceneInstance _sceneInstance;
        
        [Header("Broadcasting on")]
        [SerializeField] 
        private VoidEventChannel _onNewLayoutLoadedEventChannel;
        
        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;

        public UnityEvent OnStartLoadingLayout;
        public UnityEvent OnLayoutLoaded;
        public UnityEvent OnLayoutReady;
        public UnityEvent<LayoutSO[], LayoutSO> OnStartLayoutSelectionWheel;

        private LayoutSO _currentLayout;

        private bool _layoutLoaded;
        private int _currentSceneIndex;

        private void OnDestroy()
        {
            UnloadCurrentLayout();
        }

        [ContextMenu("Load Layout")]
        public LayoutSO LoadNewLayout()
        {
            if (_layoutSettings.LayoutCategories.Any(x => x.LayoutSO.Any(x => x == null)))
            {
                Debug.LogError($"You have a null Layout SO in your Layout Settings name {_layoutSettings.name}.");
                return null;
            }
            
            if (_remainingPlayersContainer.RemainingPlayersCount == 0)
            {
                Debug.LogError("Can't select a layout for 0 players.");
                return null;
            }
            
            OnStartLoadingLayout?.Invoke();
            
            if (_layoutLoaded)
            {
                UnloadCurrentLayout();
            }

            var layout = GetNewLayout(_remainingPlayersContainer.RemainingPlayersCount);
            
            LoadLayout(layout);
            return _currentLayout;
        }

        private LayoutSO GetNewLayout(int _playerAmount)
        {
            var availableLayoutCategories = _layoutSettings.LayoutCategories.Where(x => _playerAmount >= x.MinMaxRange.x && _playerAmount <= x.MinMaxRange.y).ToArray();

            if (availableLayoutCategories.Length == 0)
            {
                Debug.LogWarning($"No category available for {_playerAmount} players.");
                return null;
            }

            HashSet<LayoutSO> _availableLayouts = new HashSet<LayoutSO>();
            foreach (var availableLayoutCategory in availableLayoutCategories)
            {
                foreach (var layoutSo in availableLayoutCategory.LayoutSO)
                {
                    if (layoutSo == _currentLayout) continue;
                    _availableLayouts.Add(layoutSo);
                }
            }

            var selectedLayout = GetRandomLayout(_availableLayouts.ToArray());
            
            OnStartLayoutSelectionWheel?.Invoke(_availableLayouts.ToArray(), selectedLayout);
            return selectedLayout;
        }
        
        private LayoutSO GetRandomLayout(LayoutSO[] _availableLayouts)
        {
            var randomIndex = Random.Range(0, _availableLayouts.Length);
            return _availableLayouts[randomIndex];
        }
        
        private void LoadLayout(LayoutSO _layout)
        {
            if (_layout == null)
            {
                Debug.LogError($"The Layout SO reference is empty. Can't load the Layout.");
                return;
            }
            
            Debug.Log($"Loading {_layout.name}.");

            if (_currentLayout == _layout)
            {
                LayoutLoaded();
                return;
            }
            
            _currentLayout = _layout;
            _loadHandle = _layout.SceneAssetRef.LoadSceneAsync(LoadSceneMode.Additive);
            _loadHandle.Completed += handle =>
            {
                Debug.Log($"{_layout.name} loaded.");
                _sceneInstance = handle.Result;
                LayoutLoaded();
            };
        }

        private void LayoutLoaded()
        {
            OnLayoutLoaded?.Invoke();
            _onNewLayoutLoadedEventChannel.RaiseEvent();
            _layoutLoaded = true;
            OnLayoutReady?.Invoke();
        }

        public void UnloadCurrentLayout()
        {
            Debug.Log($"Unloading current scene {_sceneInstance.Scene.name}.");
            Addressables.UnloadSceneAsync(_sceneInstance).Completed += _handle =>
            {
                _layoutLoaded = false;
            };
        }
    }
}