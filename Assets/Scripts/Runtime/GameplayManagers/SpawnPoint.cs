using Gameplay.Character;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameplayManagers
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] 
        private Collider _collider;

        [SerializeField] private AssetReferenceT<VoidEventChannel> _activateBlockerChannelAssetRef;
        [SerializeField] private AssetReferenceT<VoidEventChannel> _deactivateBlockerChannelAssetRef;
        
        private PlayerRespawn _player;

        private bool _assigned;
        private AsyncOperationHandle<VoidEventChannel> _activateBlockerChannelLoadHandle;
        private AsyncOperationHandle<VoidEventChannel> _deactivateBlockerChannelLoadHandle;

        private void Awake()
        {
            _activateBlockerChannelLoadHandle = _activateBlockerChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _activateBlockerChannelLoadHandle.Completed += _handle => _handle.Result.onEventRaised += EnableCollider;
            
            _deactivateBlockerChannelLoadHandle = _deactivateBlockerChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _deactivateBlockerChannelLoadHandle.Completed += _handle => _handle.Result.onEventRaised += DisableCollider;
        }

        private void OnDestroy()
        {
            _activateBlockerChannelLoadHandle.Result.onEventRaised -= EnableCollider;
            _deactivateBlockerChannelLoadHandle.Result.onEventRaised -= DisableCollider;
            
            Addressables.Release(_activateBlockerChannelLoadHandle);
            Addressables.Release(_deactivateBlockerChannelLoadHandle);
        }

        public void AssignPlayer(PlayerRespawn _player)
        {
            this._player = _player;
            _assigned = true;
        }

        private void EnableCollider()
        {
            _collider.enabled = true;
        }

        private void DisableCollider()
        {
            _collider.enabled = false;
        }
        
        public PlayerRespawn Player => _player;

        public bool Assigned => _assigned;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, .5f);
        }
    }
}