using System.Collections;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace SceneManagementSystem.Scripts
{
    public class SceneInitializationController : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<FadeChannel> _fadeRequestChannel = default;
        [SerializeField] private FloatVariable _fadeDuration;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannel[] _initializedReportEventChannels;
        
        [Header("Broadcasting on")] [SerializeField]
        private VoidEventChannel _onSceneInitializedEventChannel;

        public UnityEvent _onSceneInitialized;
        
        private FadeChannel _fadeChannel;
        private int _awaitedEventCount;
        private AsyncOperationHandle<FadeChannel> _loadHandle;

        private void Awake()
        {
            _loadHandle = _fadeRequestChannel.LoadAssetAsync<FadeChannel>();
            _loadHandle.Completed += _handle =>
            {
                _fadeChannel = _handle.Result;
            };
            
            if (_initializedReportEventChannels.Length == 0)
            {
                SceneInitialized();
                return;
            }

            foreach (var e in _initializedReportEventChannels)
            {
                e.onEventRaised += OnAwaitedEventReport;
            }
        }

        private void OnDestroy()
        {
            Addressables.Release(_loadHandle);
            if (_initializedReportEventChannels.Length == 0) return;
            
            foreach (var e in _initializedReportEventChannels)
            {
                e.onEventRaised -= OnAwaitedEventReport;
            }
        }

        private void OnAwaitedEventReport()
        {
            _awaitedEventCount++;
            if (_awaitedEventCount < _initializedReportEventChannels.Length) return;
            SceneInitialized();
        }

        private void SceneInitialized()
        {
            _onSceneInitializedEventChannel.RaiseEvent();
            _onSceneInitialized?.Invoke();
            StartCoroutine(FadeSceneIn());
        }

        private IEnumerator FadeSceneIn()
        {
            while (_fadeChannel == null)
            {
                yield return null;
            }
            
            _fadeChannel.FadeIn(_fadeDuration.Value);
        }
    }
}