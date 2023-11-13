using Gameplay.Character;
using GeneralScriptableObjects;
using ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameplayManagers
{
    public class PlayerTurnManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<PlayerInfoEventChannel> _onPlayerTurnCompleteChannelAssetRef;
        
        [SerializeField] 
        private IntVariable _playerTurnCompleteCount;
        
        public UnityAction<Player> onPlayerTurnComplete;
        
        private AsyncOperationHandle<PlayerInfoEventChannel> _loadHandle;
        private PlayerInfoEventChannel _onPlayerTurnCompleteEventChannel;
        
        private void Awake()
        {
            _loadHandle = _onPlayerTurnCompleteChannelAssetRef.LoadAssetAsync<PlayerInfoEventChannel>();
            _loadHandle.Completed += _handle =>
            {
                _onPlayerTurnCompleteEventChannel = _handle.Result;
                _onPlayerTurnCompleteEventChannel.onEventRaised += OnPlayerTurnComplete;
            };
        }

        private void OnDestroy()
        {
            _onPlayerTurnCompleteEventChannel.onEventRaised -= OnPlayerTurnComplete;
            Addressables.Release(_loadHandle);
        }

        private void Start()
        {
            Reset();
        }

        private void OnPlayerTurnComplete(Player _player)
        {
            _playerTurnCompleteCount.SetValue(_playerTurnCompleteCount.Value + 1);
            onPlayerTurnComplete?.Invoke(_player);
        }
        
        public void Reset()
        {
            _playerTurnCompleteCount.SetValue(0);
        }
    }
}