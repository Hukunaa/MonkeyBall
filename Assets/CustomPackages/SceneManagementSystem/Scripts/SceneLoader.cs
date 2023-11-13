using System.Collections;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using SceneManagementSystem.Scripts.ScriptableObjects;
using SceneManagementSystem.Scripts.ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneManagementSystem.Scripts
{
	/// <summary>
	/// This class manages the scene loading and unloading.
	/// </summary>
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField] private GameSceneSO _gameplayScene = default;
		[SerializeField] private FloatReference _fadeDuration;
	
		[Header("Listening to")]
		[SerializeField] private LoadEventChannel _loadLocation = default;
		[SerializeField] private LoadEventChannel _loadMenu = default;
		[SerializeField] private LoadEventChannel _coldStartupLocation = default;

		[Header("Broadcasting on")]
		[SerializeField] private VoidEventChannel _onSceneReady; //picked up by the SpawnSystem
		[SerializeField] private AssetReferenceT<FadeChannel> _fadeRequestChannelAssetRef = default;

		private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
		private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

		private FadeChannel _fadeChannel;

		//Parameters coming from scene loading requests
		private GameSceneSO _sceneToLoad;
		private GameSceneSO _currentlyLoadedScene;

		private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
		private bool _isLoading = false; //To prevent a new loading request while already loading a new scene
		private AsyncOperationHandle<FadeChannel> _loadHandle;

		private void Awake()
		{
			Application.targetFrameRate = 60;
			_loadHandle = _fadeRequestChannelAssetRef.LoadAssetAsync<FadeChannel>();
			_loadHandle.Completed += _handle => _fadeChannel = _handle.Result;
		}

		private void OnDestroy()
		{
			Addressables.Release(_loadHandle);
		}

		private void OnEnable()
		{
			_loadLocation.onLoadingRequested += LoadLocation;
			_loadMenu.onLoadingRequested += LoadMenu;
#if UNITY_EDITOR
			_coldStartupLocation.onLoadingRequested += LocationColdStartup;
#endif
		}

		private void OnDisable()
		{
			_loadLocation.onLoadingRequested -= LoadLocation;
			_loadMenu.onLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
			_coldStartupLocation.onLoadingRequested -= LocationColdStartup;
#endif
		}

#if UNITY_EDITOR
		/// <summary>
		/// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialisation.
		/// </summary>
		private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
		{
			_currentlyLoadedScene = currentlyOpenedLocation;

			if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
			{
				if (SceneManager.GetSceneByName("Gameplay").isLoaded)
				{
					StartGameplay();
					return;
				}

				//Gameplay managers is loaded synchronously
				_gameplayManagerLoadingOpHandle =
					_gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
				_gameplayManagerLoadingOpHandle.Completed += _handle =>
				{
					_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;
					StartGameplay();
				};
			}
		}
#endif

		/// <summary>
		/// This function loads the location scenes passed as array parameter
		/// </summary>
		private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
		{
			//Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
			if (_isLoading)
				return;

			_sceneToLoad = locationToLoad;
			_isLoading = true;
			
			//In case we are coming from the main menu, we need to load the Gameplay manager scene first
			if (_gameplayManagerSceneInstance.Scene == null
			    || !_gameplayManagerSceneInstance.Scene.isLoaded)
			{
				_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
				_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
			}
			else
			{
				StartCoroutine(UnloadPreviousScene());
			}
		}

		private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
		{
			_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

			StartCoroutine(UnloadPreviousScene());
		}

		/// <summary>
		/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
		/// </summary>
		private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
		{
			//Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
			if (_isLoading)
				return;

			_sceneToLoad = menuToLoad;
			_isLoading = true;

			//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
			if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
			{
				Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
			}
		
			StartCoroutine(UnloadPreviousScene());
		}

		/// <summary>
		/// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
		/// </summary>
		private IEnumerator UnloadPreviousScene()
		{
			while (!_loadHandle.IsDone)
			{
				yield return null;
			}
			
			_fadeChannel.FadeOut(_fadeDuration.Value);

			yield return new WaitForSecondsRealtime(_fadeDuration.Value);

			if (_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
			{
				if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
				{
					//Unload the scene through its AssetReference, i.e. through the Addressable system
					_currentlyLoadedScene.sceneReference.UnLoadScene();
				}
#if UNITY_EDITOR
				else
				{
					//Only used when, after a "cold start", the player moves to a new scene
					//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
					//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
					SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
				}
#endif
			}

			LoadNewScene();
		}

		/// <summary>
		/// Kicks off the asynchronous loading of a scene, either menu or Location.
		/// </summary>
		private void LoadNewScene()
		{
			_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 100);
			_loadingOperationHandle.Completed += OnNewSceneLoaded;
		}

		private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
		{
			_currentlyLoadedScene = _sceneToLoad;
			
			_isLoading = false;
			Scene s = obj.Result.Scene;
			SceneManager.SetActiveScene(s);
			
			StartGameplay();
		}

		private void StartGameplay()
		{
			_onSceneReady.RaiseEvent();
		}
	}
}
