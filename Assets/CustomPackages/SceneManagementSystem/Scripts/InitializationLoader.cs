using System;
using SceneManagementSystem.Scripts.ScriptableObjects;
using SceneManagementSystem.Scripts.ScriptableObjects.EventChannels;
using TutorialSystem.Scripts.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneManagementSystem.Scripts
{
	/// <summary>
	/// This class is responsible for starting the game by loading the persistent managers scene 
	/// and raising the event to load the Main Menu
	/// </summary>

	public class InitializationLoader : MonoBehaviour
	{
		[SerializeField] 
		private GameSceneSO _managersScene = default;
		
		[SerializeField] 
		private GameSceneSO _menuScene = default;
		
		[SerializeField] 
		private GameSceneSO _tutorialScene = default;
		
		[SerializeField] 
		private AssetReferenceT<LoadEventChannel> _menuLoadChannel = default;
		
		[SerializeField] 
		private AssetReference _tutorialLoadChannel = default;
		
		[SerializeField] 
		private bool _checkForTutorial;

		[SerializeField] 
		private string _gameplayTutorialName;
		
		private LoadEventChannel _loadEventChannel;
		private AsyncOperationHandle<LoadEventChannel> _loadEventChannelHandle;
		
		public void StartInitialization()
		{
			Initialize();
		}

		private void OnDestroy()
		{
			if (!_loadEventChannelHandle.IsValid()) return;
			Addressables.Release(_loadEventChannelHandle);
		}

		public void Initialize()
		{
			_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadFirstScene;
		}

		private void LoadFirstScene(AsyncOperationHandle<SceneInstance> _handle)
		{
			LoadFirstSceneCoroutine();
		}
		
		private void LoadFirstSceneCoroutine()
		{
			if (_checkForTutorial && !TutorialDataManager.IsTutorialComplete(_gameplayTutorialName))
			{
				_loadEventChannelHandle = _tutorialLoadChannel.LoadAssetAsync<LoadEventChannel>();
				_loadEventChannelHandle.Completed += _handle =>
				{
					_loadEventChannel = _handle.Result;
					_loadEventChannel.RaiseEvent(_tutorialScene, true);
				};
			}

			else
			{
				_loadEventChannelHandle = _menuLoadChannel.LoadAssetAsync<LoadEventChannel>();
				_loadEventChannelHandle.Completed += _handle =>
				{
					_loadEventChannel = _handle.Result;
					_loadEventChannel.RaiseEvent(_menuScene, true);
				};
			}
			
			SceneManager.UnloadSceneAsync(0);
		}
	}
}
