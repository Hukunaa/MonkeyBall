using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameplayManagers
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField]
        private IntVariable _roundCount;

        [SerializeField] 
        private UnityEvent _onPrepareNewRound;
        
        [SerializeField] 
        private UnityEvent _onStartNewRound;

        [SerializeField] 
        private AssetReferenceT<VoidEventChannel> _newRoundStartEventChannelAssetRef;
        
        private AsyncOperationHandle<VoidEventChannel> _newRoundStartEventChannelLoadHandle;
        private VoidEventChannel _newRoundStartEventChannel;
        
        private void Awake()
        {
            _newRoundStartEventChannelLoadHandle = _newRoundStartEventChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _newRoundStartEventChannelLoadHandle.Completed += _handle =>
            {
                _newRoundStartEventChannel = _handle.Result;
            };
        }

        private void OnDestroy()
        {
            Addressables.Release(_newRoundStartEventChannelLoadHandle);
        }

        public void Initialize()
        {
            Reset();
            StartNewRound();
        }
        
        public void StartNewRound()
        {
            _newRoundStartEventChannelLoadHandle.WaitForCompletion();
            _roundCount.SetValue(_roundCount.Value + 1);
            _newRoundStartEventChannel.RaiseEvent();
            _onPrepareNewRound?.Invoke();
            _onStartNewRound?.Invoke();
        }

        public void Reset()
        {
            _roundCount.SetValue(0);
        }
    }
}