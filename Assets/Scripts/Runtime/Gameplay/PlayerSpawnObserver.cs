using GeneralScriptableObjects.EventChannels;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerSpawnObserver : MonoBehaviour
{
    [SerializeField] 
    private AssetReferenceT<TransformEventChannel> _onNotifyPlayerSpawnEventChannel;
    
    [SerializeField] 
    private AssetReferenceT<TransformEventChannel> _onNotifyPlayerDespawnEventChannel;

    [SerializeField] 
    private UnityEvent<Transform> _onPlayerToFollowChanged;

    [SerializeField] 
    private UnityEvent _onStartFollow;
    
    [SerializeField] 
    private UnityEvent _onStopFollow;

    [SerializeField] 
    private bool _resetPositionOnStopFollow;
    
    private AsyncOperationHandle<TransformEventChannel> _playerSpawnNotifyChannelLoadHandle;
    private AsyncOperationHandle<TransformEventChannel> _playerDespawnNotifyChannelLoadHandle;

    private void Awake()
    {
        _playerSpawnNotifyChannelLoadHandle = _onNotifyPlayerSpawnEventChannel.LoadAssetAsync<TransformEventChannel>();
        _playerSpawnNotifyChannelLoadHandle.Completed += _handle =>
        {
            _handle.Result.onEventRaised += StartFollowPlayer;
        };

        _playerDespawnNotifyChannelLoadHandle = _onNotifyPlayerDespawnEventChannel.LoadAssetAsync<TransformEventChannel>();
        _playerDespawnNotifyChannelLoadHandle.Completed += _handle =>
        {
            _handle.Result.onEventRaised += StopFollowPlayer;
        };
    }

    private void OnDestroy()
    {
        _playerSpawnNotifyChannelLoadHandle.Result.onEventRaised -= StartFollowPlayer;
        _playerDespawnNotifyChannelLoadHandle.Result.onEventRaised -= StopFollowPlayer;
        
        Addressables.Release(_playerSpawnNotifyChannelLoadHandle);
        Addressables.Release(_playerDespawnNotifyChannelLoadHandle);
    }

    private void StartFollowPlayer(Transform _playerTransform)
    {
        _onPlayerToFollowChanged?.Invoke(_playerTransform);
        _onStartFollow?.Invoke();
    }

    private void StopFollowPlayer(Transform _playerTransform)
    {
        _onStopFollow?.Invoke();
        
        if (_resetPositionOnStopFollow == false) return;
        transform.SetPositionAndRotation(Vector3.zero, quaternion.identity);
    }
}
