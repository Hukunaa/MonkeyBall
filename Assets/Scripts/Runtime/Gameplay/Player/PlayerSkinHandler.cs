using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using ScriptableObjects.DataContainer;

namespace Gameplay.Player
{
    public class PlayerSkinHandler : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter _headMesh;
        [SerializeField]
        private MeshFilter _faceMesh;
        [SerializeField]
        private MeshFilter _earSkinColorMesh;
        [SerializeField]
        private SkinnedMeshRenderer _bodySkinColorMesh;
        [SerializeField]
        private SkinnedMeshRenderer _bodyMesh;
        [SerializeField]
        private GameObject _ballParent;
        [SerializeField]
        private GameObject _gliderParent;

        private Object _currentHeadMesh;
        private Object[] _currentSkinColorMesh;
        private Object _currentFaceMesh;
        private Object _currentBodyMesh;
        private Object _currentBallMesh;
        private Object _currentGliderMesh;

        private AssetReference _oldBallReference;
        private AssetReference _oldGliderReference;

        private AsyncOperationHandle<GameObject> _ballPrefabHandle;
        private AsyncOperationHandle<GameObject> _gliderPrefabHandle;

        private UnityAction _onBallSkinUpdated;
        private UnityAction _onGliderSkinUpdated;

        public void UpdateHeadSkin(Object _mesh)
        {
            if (_currentHeadMesh == _mesh)
                return;

            _currentHeadMesh = _mesh;
            MeshFilter _meshFilter = (_mesh as GameObject).GetComponentInChildren<MeshFilter>();
            _headMesh.sharedMesh = _meshFilter.sharedMesh;
        }
        public void UpdateSkinColor(Object[] _mesh)
        {
            if (_currentSkinColorMesh != null)
                if(_currentSkinColorMesh.ToArray() == _mesh)
                    return;

            _currentSkinColorMesh = _mesh;
            SkinnedMeshRenderer _skinnedMesh = (_mesh[0] as GameObject).GetComponentInChildren<SkinnedMeshRenderer>();
            _bodySkinColorMesh.sharedMesh = _skinnedMesh.sharedMesh;
            _earSkinColorMesh.sharedMesh = (_mesh[1] as Mesh);
        }

        public void UpdateBodySkin(Object _mesh)
        {
            if (_currentBodyMesh == _mesh)
                return;

            _currentBodyMesh = _mesh;
            SkinnedMeshRenderer _skinnedMesh = (_mesh as GameObject).GetComponentInChildren<SkinnedMeshRenderer>();
            _bodyMesh.sharedMesh = _skinnedMesh.sharedMesh;
        }

        public void UpdateFaceSkin(Object _mesh)
        {
            if (_currentFaceMesh == _mesh)
                return;

            SkinData _data = (_mesh as SkinData);

            _currentFaceMesh = _mesh;
            _faceMesh.sharedMesh = (_data.MeshAssetRef as Mesh);
            _faceMesh.GetComponent<MeshRenderer>().sharedMaterial = _data.CustomMaterial;
        }

        public async void UpdateBallSkin(AssetReference _prefab)
        {
            if (_oldBallReference == _prefab)
                return;
            else
                _oldBallReference = _prefab;

            if (_currentBallMesh != null)
            {
                Destroy(_currentBallMesh as GameObject);
                Addressables.Release(_ballPrefabHandle);
            }

            _ballPrefabHandle = _prefab.InstantiateAsync(_ballParent.transform);
            GameObject instance = await _ballPrefabHandle.Task;
            _currentBallMesh = instance;
            _onBallSkinUpdated?.Invoke();
        }
        public async void UpdateGliderSkin(AssetReference _prefab)
        {
            if (_oldGliderReference == _prefab)
                return;
            else
                _oldGliderReference = _prefab;

            if (_currentGliderMesh != null)
            {
                Destroy(_currentGliderMesh as GameObject);
                Addressables.Release(_gliderPrefabHandle);
            }

            _gliderPrefabHandle = _prefab.InstantiateAsync(_gliderParent.transform);
            GameObject instance = await _gliderPrefabHandle.Task;
            _currentGliderMesh = instance;
            _onGliderSkinUpdated?.Invoke();
        }

        public Object CurrentHeadMesh => _currentHeadMesh;
        public Object CurrentFaceMesh => _currentFaceMesh;
        public Object CurrentBodyMesh => _currentBodyMesh;
        public Object CurrentBallMesh => _currentBallMesh;
        public Object CurrentGliderMesh => _currentGliderMesh;
        public Object[] CurrentSkinColorMeshes => _currentSkinColorMesh;
        public UnityAction OnBallSkinUpdated { get => _onBallSkinUpdated; set => _onBallSkinUpdated = value; }
        public UnityAction OnGliderSkinUpdated { get => _onGliderSkinUpdated; set => _onGliderSkinUpdated = value; }
        public AsyncOperationHandle<GameObject> GliderPrefabHandle { get => _gliderPrefabHandle; }
    }
}
