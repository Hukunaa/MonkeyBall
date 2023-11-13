using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Character.AI;
using Gameplay.Player;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Task = System.Threading.Tasks.Task;

namespace Utilities
{
    public class AISkinsRandomizer : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<SkinData>[] _headSkinsAssetRefs;

        [SerializeField]
        private AssetReferenceT<SkinData>[] _faceSkinsAssetRefs;

        [SerializeField]
        private AssetReferenceT<SkinData>[] _skinColorAssetRefs;

        [SerializeField] 
        private AssetReferenceT<SkinData>[] _bodySkinsAssetRef;

        [SerializeField]
        private AssetReferenceT<SkinData>[] _ballSkinsAssetRef;

        [SerializeField]
        private AssetReferenceT<SkinData>[] _gliderSkinsAssetRef;

        [SerializeField] 
        private VoidEventChannel _onPlayerSpawnedEventChannel;
        
        private List<AsyncOperationHandle<SkinData>> _headSkinHandles;
        private List<SkinData> _headSkinsData = new List<SkinData>();

        private List<AsyncOperationHandle<SkinData>> _faceSkinHandles;
        private List<SkinData> _faceSkinsData = new List<SkinData>();

        private List<AsyncOperationHandle<SkinData>> _skinColorHandles;
        private List<SkinData> _skinColorData = new List<SkinData>();

        private List<AsyncOperationHandle<SkinData>> _bodySkinHandles;
        private List<SkinData> _bodySkinsData = new List<SkinData>();

        private List<AsyncOperationHandle<SkinData>> _ballSkinHandles;
        private List<SkinData> _ballSkinsData = new List<SkinData>();

        private List<AsyncOperationHandle<SkinData>> _gliderSkinHandles;
        private List<SkinData> _gliderSkinsData = new List<SkinData>();

        private bool _skinsLoaded;

        private void Awake()
        {
            _onPlayerSpawnedEventChannel.onEventRaised += Randomize;

            LoadSkins();
        }

        private async void LoadSkins()
        {
            List<Task<AsyncOperationHandle<SkinData>>> loadHeadSkinsTasks =
                new List<Task<AsyncOperationHandle<SkinData>>>();

            List<Task<AsyncOperationHandle<SkinData>>> loadFaceSkinsTasks =
                new List<Task<AsyncOperationHandle<SkinData>>>();

            List<Task<AsyncOperationHandle<SkinData>>> loadSkinColorTasks =
                new List<Task<AsyncOperationHandle<SkinData>>>();

            List<Task<AsyncOperationHandle<SkinData>>> loadBodySkinsTasks =
                new List<Task<AsyncOperationHandle<SkinData>>>();

            List<Task<AsyncOperationHandle<SkinData>>> loadBallSkinsTasks =
                new List<Task<AsyncOperationHandle<SkinData>>>();

            List<Task<AsyncOperationHandle<SkinData>>> loadGliderSkinsTasks =
            new List<Task<AsyncOperationHandle<SkinData>>>();

            foreach (var headSkinAssetRef in _headSkinsAssetRefs)
            {
                loadHeadSkinsTasks.Add(LoadSkin(headSkinAssetRef));
            }

            foreach (var faceSkinAssetRef in _faceSkinsAssetRefs)
            {
                loadFaceSkinsTasks.Add(LoadSkin(faceSkinAssetRef));
            }

            foreach (var skinColorAssetRef in _skinColorAssetRefs)
            {
                loadSkinColorTasks.Add(LoadSkin(skinColorAssetRef));
            }

            foreach (var bodySkinAssetRef in _bodySkinsAssetRef)
            {
                loadBodySkinsTasks.Add(LoadSkin(bodySkinAssetRef));
            }

            foreach (var ballSkinAssetRef in _ballSkinsAssetRef)
            {
                loadBallSkinsTasks.Add(LoadSkin(ballSkinAssetRef));
            }

            foreach (var gliderSkinAssetRef in _gliderSkinsAssetRef)
            {
                loadGliderSkinsTasks.Add(LoadSkin(gliderSkinAssetRef));
            }

            List<Task<List<AsyncOperationHandle<SkinData>>>> assetCategoryLoadingTask =
                new List<Task<List<AsyncOperationHandle<SkinData>>>>();

            var headAssetsLoadingTask = WaitForAssetToLoad(loadHeadSkinsTasks);
            var faceAssetsLoadingTask = WaitForAssetToLoad(loadFaceSkinsTasks);
            var skinColorAssetsLoadingTask = WaitForAssetToLoad(loadSkinColorTasks);
            var bodyAssetsLoadingTask = WaitForAssetToLoad(loadBodySkinsTasks);
            var ballAssetsLoadingTask = WaitForAssetToLoad(loadBallSkinsTasks);
            var gliderAssetsLoadingTask = WaitForAssetToLoad(loadGliderSkinsTasks);

            assetCategoryLoadingTask.Add(headAssetsLoadingTask);
            assetCategoryLoadingTask.Add(faceAssetsLoadingTask);
            assetCategoryLoadingTask.Add(skinColorAssetsLoadingTask);
            assetCategoryLoadingTask.Add(bodyAssetsLoadingTask);
            assetCategoryLoadingTask.Add(ballAssetsLoadingTask);
            assetCategoryLoadingTask.Add(gliderAssetsLoadingTask);

            while (assetCategoryLoadingTask.Count > 0)
            {
                var finishedTask = await Task.WhenAny(assetCategoryLoadingTask);
                if (finishedTask == headAssetsLoadingTask)
                {
                    _headSkinHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _headSkinsData.Add(asyncOperationHandle.Result);
                    }
                }

                else if (finishedTask == faceAssetsLoadingTask)
                {
                    _faceSkinHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _faceSkinsData.Add(asyncOperationHandle.Result);
                    }
                }

                else if (finishedTask == skinColorAssetsLoadingTask)
                {
                    _skinColorHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _skinColorData.Add(asyncOperationHandle.Result);
                    }
                }

                else if (finishedTask == bodyAssetsLoadingTask)
                {
                    _bodySkinHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _bodySkinsData.Add(asyncOperationHandle.Result);
                    }
                }

                else if (finishedTask == ballAssetsLoadingTask)
                {
                    _ballSkinHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _ballSkinsData.Add(asyncOperationHandle.Result);
                    }
                }

                else if (finishedTask == gliderAssetsLoadingTask)
                {
                    _gliderSkinHandles = finishedTask.Result;
                    foreach (var asyncOperationHandle in finishedTask.Result)
                    {
                        _gliderSkinsData.Add(asyncOperationHandle.Result);
                    }
                }

                assetCategoryLoadingTask.Remove(finishedTask);
            }

            _skinsLoaded = true;
        }

        private async Task<List<AsyncOperationHandle<SkinData>>> WaitForAssetToLoad(List<Task<AsyncOperationHandle<SkinData>>> _loadingTasks)
        {
            List<AsyncOperationHandle<SkinData>> loadHandles = new List<AsyncOperationHandle<SkinData>>();
            
            while (_loadingTasks.Count > 0)
            {
                Task<AsyncOperationHandle<SkinData>> finishedTask = await Task.WhenAny(_loadingTasks);
                loadHandles.Add(finishedTask.Result);
                _loadingTasks.Remove(finishedTask);
            }

            return loadHandles;
        }

        private async Task<AsyncOperationHandle<SkinData>> LoadSkin(AssetReferenceT<SkinData> _assetRef)
        {
            var handle = _assetRef.LoadAssetAsync<SkinData>();
            await handle.Task;
            return handle;
        }

        private void OnDestroy()
        {
            _onPlayerSpawnedEventChannel.onEventRaised -= Randomize;
            
            foreach (var handle in _headSkinHandles)
            {
                Addressables.Release(handle);
            }

            foreach (var handle in _faceSkinHandles)
            {
                Addressables.Release(handle);
            }

            foreach (var handle in _skinColorHandles)
            {
                Addressables.Release(handle);
            }

            foreach (var handle in _bodySkinHandles)
            {
                Addressables.Release(handle);
            }

            foreach (var handle in _ballSkinHandles)
            {
                Addressables.Release(handle);
            }

            foreach (var handle in _gliderSkinHandles)
            {
                Addressables.Release(handle);
            }
        }
        
        private void Randomize()
        {
            StartCoroutine(RandomizeSkinsCoroutine());
        }

        private IEnumerator RandomizeSkinsCoroutine()
        {
            var skinHandlers = FindAIPlayerSkinsLoader();

            while (_skinsLoaded == false)
            {
                yield return null;
            }
            
            foreach (var playerSkinsHandle in skinHandlers)
            {
                playerSkinsHandle.UpdateHeadSkin(GetRandomHeadMesh());
                playerSkinsHandle.UpdateFaceSkin(GetRandomFaceMesh());
                playerSkinsHandle.UpdateSkinColor(GetRandomSkinColorMesh());
                playerSkinsHandle.UpdateBodySkin(GetRandomBodyMesh());
                playerSkinsHandle.UpdateBallSkin(GetRandomBallMesh());
                playerSkinsHandle.UpdateGliderSkin(GetRandomGliderMesh());
            }
        }

        private PlayerSkinHandler[] FindAIPlayerSkinsLoader()
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            List<PlayerSkinHandler> playerSkinLoaders = new List<PlayerSkinHandler>();

            foreach (var player in players)
            {
                if (player.GetComponent<AIController>() != null)
                {
                    playerSkinLoaders.Add(player.GetComponent<PlayerSkinHandler>());
                }
            }

            return playerSkinLoaders.ToArray();
        }

        private Object GetRandomHeadMesh()
        {
            var randomSkinIndex = Random.Range(0, _headSkinsData.Count);
            return _headSkinsData[randomSkinIndex].MeshAssetRef;
        } 
        
        private Object GetRandomBodyMesh()
        {
            var randomSkinIndex = Random.Range(0, _bodySkinsData.Count);
            return _bodySkinsData[randomSkinIndex].MeshAssetRef;
        }
        private Object GetRandomFaceMesh()
        {
            var randomSkinIndex = Random.Range(0, _faceSkinsData.Count);
            return _faceSkinsData[randomSkinIndex];
        }

        private AssetReference GetRandomBallMesh()
        {
            var randomSkinIndex = Random.Range(0, _ballSkinsData.Count);
            return _ballSkinsData[randomSkinIndex].BallSkinRef;
        }

        private AssetReference GetRandomGliderMesh()
        {
            var randomSkinIndex = Random.Range(0, _gliderSkinsData.Count);
            return _gliderSkinsData[randomSkinIndex].GliderSkinRef;
        }
        private Object[] GetRandomSkinColorMesh()
        {
            var randomSkinIndex = Random.Range(0, _skinColorData.Count);
            return _skinColorData[randomSkinIndex].SkinColorAssetRefs;
        }
    }
}