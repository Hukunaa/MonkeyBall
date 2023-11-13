using System;
using UI;
using UI.GameplayUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay.Character
{
    public class AIPlayerInstantiationManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<GameObject> _pointsEarnedAssetRef;
        
        private AsyncOperationHandle<GameObject> _loadHandle;
        private PlayerNameUI _playerNameUI;

        private void Awake()
        {
            _loadHandle = _pointsEarnedAssetRef.LoadAssetAsync<GameObject>();
            _loadHandle.Completed += LoadHandleOnCompleted;
        }

        private void OnDestroy()
        {
            Addressables.Release(_loadHandle);
        }

        private void LoadHandleOnCompleted(AsyncOperationHandle<GameObject> obj)
        {
            var instance = Instantiate(obj.Result);
            _playerNameUI = instance.GetComponent<PlayerNameUI>();
            _playerNameUI.SetPlayerInfo(transform.GetComponent<Player>());
        }
    }
}