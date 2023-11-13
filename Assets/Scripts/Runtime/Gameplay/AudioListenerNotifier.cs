using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioListenerNotifier : MonoBehaviour
{
    [SerializeField] 
    private AssetReferenceT<TransformEventChannel> _onNotifyPlayerSpawnEventChannel;
    
    [SerializeField] 
    private AssetReferenceT<TransformEventChannel> _onNotifyPlayerDespawnEventChannel;

    private AsyncOperationHandle<TransformEventChannel> _playerSpawnNotifyChannelLoadHandle;
    private AsyncOperationHandle<TransformEventChannel> _playerDespawnNotifyChannelLoadHandle;

    private void Awake()
    {
        _playerSpawnNotifyChannelLoadHandle = _onNotifyPlayerSpawnEventChannel.LoadAssetAsync<TransformEventChannel>();
        _playerSpawnNotifyChannelLoadHandle.Completed += _handle =>
        {
            _handle.Result.RaiseEvent(transform);
            Addressables.Release(_playerSpawnNotifyChannelLoadHandle);
        };

        _playerDespawnNotifyChannelLoadHandle = _onNotifyPlayerDespawnEventChannel.LoadAssetAsync<TransformEventChannel>();
    }

    private void OnDestroy()
    {
        _playerDespawnNotifyChannelLoadHandle.Result.RaiseEvent(transform);
        Addressables.Release(_playerDespawnNotifyChannelLoadHandle);
    }
}
