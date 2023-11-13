using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjects.DataContainer;
using ScriptableObjects.EventChannels;
using TMPro;
using UIPackage.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI.MainMenu.StoreUI
{
    public class CategoryManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<GameObject> _storeItemAssetRef;

        [SerializeField] 
        private TMP_Text _categoryNameTmp;

        [SerializeField] 
        private Transform _storeItemsParent;

        [SerializeField] 
        private AssetReferenceT<SkinDataEventChannel> _onSkinBoughtEventChannelAssetRef;

        private HashSet<SkinItem> _skinItems = new HashSet<SkinItem>();
        private AsyncOperationHandle<GameObject> _loadHandle;
        private GameObject _storeItemPrefab;

        private StoreItem _currentItem;
        private AsyncOperationHandle<SkinDataEventChannel> _onSkinBoughtEventChannelLoadHandle;
        private SkinDataEventChannel _onSkinBoughtEventChannel;

        private void Awake()
        {
            _onSkinBoughtEventChannelLoadHandle = _onSkinBoughtEventChannelAssetRef.LoadAssetAsync<SkinDataEventChannel>();
            _onSkinBoughtEventChannelLoadHandle.Completed += _handle =>
            {
                _onSkinBoughtEventChannel = _handle.Result;
                _onSkinBoughtEventChannel.onEventRaised += OnSkinBought;
            };
        }

        private void OnDestroy()
        {
            Addressables.Release(_loadHandle);
            Addressables.Release(_onSkinBoughtEventChannelLoadHandle);
            
            _onSkinBoughtEventChannel.onEventRaised -= OnSkinBought;
        }
    
        public IEnumerator InitializeCategoryCoroutine(ESkinType _skinType, HashSet<SkinData> _skins)
        {
            _categoryNameTmp.text = _skinType.ToString();
            _loadHandle = _storeItemAssetRef.LoadAssetAsync<GameObject>();
            yield return _loadHandle;
            _storeItemPrefab = _loadHandle.Result;

            foreach (var skinData in _skins)
            {
                CreateSkinItem(skinData);
            }
        }

        private void CreateSkinItem(SkinData skinData)
        {
            var item = Instantiate(_storeItemPrefab, _storeItemsParent).GetComponent<SkinItem>();
            item.Initialize(skinData);
            _skinItems.Add(item);
        }

        private void RemoveStoreItem(SkinItem _skinItem)
        {
            _skinItems.Remove(_skinItem);
            Destroy(_skinItem.gameObject);

            if (_skinItems.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnSkinBought(SkinData _skinData)
        {
            var item = _skinItems.FirstOrDefault(x => x.SkinData == _skinData);

            if (item != null)
            {
                RemoveStoreItem(item);
            }
        }
    }
}
