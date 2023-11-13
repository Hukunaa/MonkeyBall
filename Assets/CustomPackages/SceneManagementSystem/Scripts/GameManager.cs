using System.Threading.Tasks;
using GeneralScriptableObjects.EventChannels;
using Runtime.ScriptableObjects.DataContainers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SceneManagementSystem.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance = null;
        public static GameManager Instance => _instance;
        
        [SerializeField] 
        private AssetReferenceT<PlayerDataContainer> _playerDataContainerAssetRef;

        [SerializeField] 
        private AssetReferenceT<VoidEventChannel> _playerDataLoadedEventChannel;
        
        private PlayerDataContainer _playerDataContainer;
        private AsyncOperationHandle<PlayerDataContainer> _playerDataContainerLoadHandle;
        private AsyncOperationHandle<VoidEventChannel> _onPlayerDataLoadedHandle;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _instance = this;
            }
            LoadDataContainers();
            TinySauce.OnGameStarted();
        }
        
        private async void LoadDataContainers()
        {
            _playerDataContainerLoadHandle = _playerDataContainerAssetRef.LoadAssetAsync<PlayerDataContainer>();
            _playerDataContainerLoadHandle.Completed += _handle => _playerDataContainer = _handle.Result;

            while (!_playerDataContainerLoadHandle.IsDone)
            {
                await Task.Delay(50);
            }

            await _playerDataContainer.LoadPlayerData();
            
            DataLoaded = true;
            _onPlayerDataLoadedHandle = _playerDataLoadedEventChannel.LoadAssetAsync<VoidEventChannel>();
            _onPlayerDataLoadedHandle.Completed += _handle => _handle.Result.RaiseEvent();
        }

        private void OnDestroy()
        {
            TinySauce.OnGameFinished(0);
            Addressables.Release(_playerDataContainerLoadHandle);
            Addressables.Release(_onPlayerDataLoadedHandle);
        }
        
        
        public PlayerDataContainer PlayerDataContainer => _playerDataContainer;

        public bool DataLoaded { get; private set; }
    }
}
