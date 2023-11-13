using SceneManagementSystem.Scripts.ScriptableObjects;
using SceneManagementSystem.Scripts.ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SceneManagementSystem.Scripts
{
	public class SceneController : MonoBehaviour
	{
		[SerializeField] 
		private bool _showLoadScreen;

		[SerializeField] 
		private AssetReferenceT<LoadEventChannel> _loadGameSceneEventChannelAssetRef;
		
		[SerializeField] 
		private AssetReferenceT<LoadEventChannel> _loadMenuEventChannelAssetRef;
		
		[SerializeField] 
		private UnityEvent _onLeaveScene;

		private AsyncOperationHandle<LoadEventChannel> loadGameSceneEventChannelLoadHandle;
		private AsyncOperationHandle<LoadEventChannel> loadMenuEventChannelLoadHandle;
		
		private LoadEventChannel _loadGameSceneEventChannel;
		private LoadEventChannel _loadMenuEventChannel;

		private void Awake()
		{
			loadGameSceneEventChannelLoadHandle = _loadGameSceneEventChannelAssetRef.LoadAssetAsync<LoadEventChannel>();
			loadGameSceneEventChannelLoadHandle.Completed += handle => _loadGameSceneEventChannel = handle.Result;
			
			loadMenuEventChannelLoadHandle = _loadMenuEventChannelAssetRef.LoadAssetAsync<LoadEventChannel>();
			loadMenuEventChannelLoadHandle.Completed += _handle => _loadMenuEventChannel = _handle.Result;
		}

		private void OnDestroy()
		{
			Addressables.Release(loadGameSceneEventChannelLoadHandle);
			Addressables.Release(loadMenuEventChannelLoadHandle);
		}

		public void LoadScene(LocationSO _sceneToLoad)
		{
			_onLeaveScene?.Invoke();
			_loadGameSceneEventChannel.RaiseEvent(_sceneToLoad, _showLoadScreen);
		}

		public void LoadMenu(GameSceneSO _menuToLoad)
		{
			_onLeaveScene?.Invoke();
			_loadMenuEventChannel.RaiseEvent(_menuToLoad, _showLoadScreen);
		}
	}
}
