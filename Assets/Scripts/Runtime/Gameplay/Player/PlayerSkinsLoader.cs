using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;

namespace Gameplay.Player
{
    public class PlayerSkinsLoader : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent<Object> _onUpdateHeadMesh;
        
        [SerializeField] 
        private UnityEvent<Object> _onUpdateBodyMesh;

        [SerializeField]
        private UnityEvent<AssetReference> _onUpdateBallMesh;

        [SerializeField]
        private UnityEvent<AssetReference> _onUpdateGliderMesh;

        [SerializeField]
        private UnityEvent<Object> _onUpdateFaceMesh;

        [SerializeField]
        private UnityEvent<Object[]> _onUpdateSkinColorMeshes;

        private List<SkinData> _skins;
        private AsyncOperationHandle<IList<SkinData>> _skinDataLoadHandle;
        private AsyncOperationHandle<SkinData> _skinPreviewLoadHandle;

        private void Awake()
        {
            _skins = new List<SkinData>();
            LoadSkinFromCurrentData();
        }
        
        private void OnDestroy()
        {
            if (_skinDataLoadHandle.IsValid())
                Addressables.Release(_skinDataLoadHandle);

            if (_skinPreviewLoadHandle.IsValid())
                Addressables.Release(_skinPreviewLoadHandle);
        }

        public async void LoadSkinFromCurrentData()
        {
            await LoadSkinsData();
            _onUpdateHeadMesh?.Invoke(_skins.First(x => x.SkinType == ESkinType.HEAD).MeshAssetRef);
            _onUpdateBodyMesh?.Invoke(_skins.First(x => x.SkinType == ESkinType.BODY).MeshAssetRef);
            _onUpdateBallMesh?.Invoke(_skins.First(x => x.SkinType == ESkinType.BALL).BallSkinRef);
            _onUpdateGliderMesh?.Invoke(_skins.First(x => x.SkinType == ESkinType.GLIDER).GliderSkinRef);
            _onUpdateFaceMesh?.Invoke(_skins.First(x => x.SkinType == ESkinType.FACE));
            _onUpdateSkinColorMeshes?.Invoke(_skins.First(x => x.SkinType == ESkinType.COLOR).SkinColorAssetRefs);
        }

        public async void LoadPreviewSkin(KeyValuePair<string, int> _skin)
        {
            if (_skinPreviewLoadHandle.IsValid())
                Addressables.Release(_skinPreviewLoadHandle);

            string _asset = $"{AddressableLoadingUtilities.SkinsAddressablePath}{_skin.Key}.asset";

            _skinPreviewLoadHandle = Addressables.LoadAssetAsync<SkinData>(_asset);
            await _skinPreviewLoadHandle.Task;
            SkinData _skinData = _skinPreviewLoadHandle.Result;
            Debug.Log("Loading Skin");
            switch(_skin.Value)
            {
                case 0:
                    _onUpdateHeadMesh?.Invoke(_skinData.MeshAssetRef);
                    break;
                case 1:
                    _onUpdateBodyMesh?.Invoke(_skinData.MeshAssetRef);
                    break;
                case 2:
                    _onUpdateBallMesh?.Invoke(_skinData.BallSkinRef);
                    break;
                case 3:
                    _onUpdateGliderMesh?.Invoke(_skinData.GliderSkinRef);
                    break;
                case 4:
                    _onUpdateFaceMesh?.Invoke(_skinData);
                    break;
                case 5:
                    _onUpdateSkinColorMeshes?.Invoke(_skinData.SkinColorAssetRefs);
                    break;
            }
        }

        private async Task LoadSkinsData()
        {
            List<string> keys = DataLoader.LoadPlayerCurrentSkins();
            IEnumerable<string> skinKeys = keys.Select(x => $"{AddressableLoadingUtilities.SkinsAddressablePath}{x}.asset").AsEnumerable();

            if (_skins.Count > 0)
                _skins.Clear();

            _skinDataLoadHandle = Addressables.LoadAssetsAsync<SkinData>(skinKeys, obj => { _skins.Add(obj); }, Addressables.MergeMode.Union);
            await _skinDataLoadHandle.Task;
        }

        public UnityEvent<AssetReference> OnUpdateBallMesh { get => _onUpdateBallMesh; }
        public UnityEvent<AssetReference> OnUpdateGliderMesh { get => _onUpdateGliderMesh; }
        public UnityEvent<Object> OnUpdateBodyMesh { get => _onUpdateBodyMesh; }
        public UnityEvent<Object> OnUpdateHeadMesh { get => _onUpdateHeadMesh; }
        public UnityEvent<Object> OnUpdateFaceMesh { get => _onUpdateFaceMesh; }
        public UnityEvent<Object[]> OnUpdateSkinColorMeshes { get => _onUpdateSkinColorMeshes; }
    }
}