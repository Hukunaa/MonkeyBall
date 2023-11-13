using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Utilities;

namespace UI.MainMenu.StoreUI
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<GameObject> _itemCategoryAssetRef;

        private AsyncOperationHandle<GameObject> itemCategoryLoadHandle;

        private GameObject _itemCategoryPrefab;

        [FormerlySerializedAs("_categoryParents")] [SerializeField] 
        private Transform _categoriesParent;

        [SerializeField] 
        private List<SkinData> _storeSkins;

        [SerializeField] 
        private UnityEvent _onStoreInitialized;
    
        private List<SkinData> _headSkinsData;
        private List<SkinData> _bodySkinsData;
        private List<SkinData> _ballSkinsData;
        private List<SkinData> _gliderSkinsData;

        private Dictionary<ESkinType, HashSet<SkinData>> _skins = new Dictionary<ESkinType, HashSet<SkinData>>();

        private AsyncOperationHandle<IList<SkinData>> _skinDataLoadHandle;

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private void OnDestroy()
        {
            if (_skinDataLoadHandle.IsValid() == false) return;
            Addressables.Release(_skinDataLoadHandle);
        }

        private IEnumerator Initialize()
        {
            var storeSkins = DataLoader.LoadSkins(DataLoader.PersistentStoreSkinsDataPath);
            var inventorySkins = DataLoader.LoadSkins(DataLoader.PersistentPlayerSkinsInventoryDataPath);

            //Remove skins already owned by the player
            for (int i = storeSkins.Count - 1; i >= 0; i--)
            {
                if (inventorySkins.Contains(storeSkins[i]))
                {
                    storeSkins.RemoveAt(i);
                }
            }
        
            if (storeSkins.Count != 0)
            {
                yield return StartCoroutine(LoadSkinsData(storeSkins));
            
                RangeSkinsByCategory();

                yield return StartCoroutine(LoadCategoryPrefab());

                yield return StartCoroutine(CreateCategories());
            }
        
            _onStoreInitialized?.Invoke();
        }

        private IEnumerator CreateCategories()
        {
            foreach (var skinCategory in _skins)
            {
                var category = Instantiate(_itemCategoryPrefab, _categoriesParent).GetComponent<CategoryManager>();
                yield return StartCoroutine(category.InitializeCategoryCoroutine(skinCategory.Key, skinCategory.Value));
            }
        }

        private IEnumerator LoadCategoryPrefab()
        {
            itemCategoryLoadHandle = _itemCategoryAssetRef.LoadAssetAsync<GameObject>();
            yield return itemCategoryLoadHandle;
            _itemCategoryPrefab = itemCategoryLoadHandle.Result;
        }

        private void RangeSkinsByCategory()
        {
            foreach (var storeSkin in _storeSkins)
            {
                if (!_skins.ContainsKey(storeSkin.SkinType))
                {
                    _skins.Add(storeSkin.SkinType, new HashSet<SkinData> { storeSkin });
                }

                else
                {
                    _skins[storeSkin.SkinType].Add(storeSkin);
                }
            }
        }

        private IEnumerator LoadSkinsData(List<string> _keys)
        {
            var skinKeys =  _keys.Select(x => $"{AddressableLoadingUtilities.SkinsAddressablePath}{x}.asset").AsEnumerable();
            yield return AddressableLoadingUtilities.LoadAddressableByKeysCoroutine<SkinData>(skinKeys, OnSkinsSOLoaded);
        }

        private void OnSkinsSOLoaded(AsyncOperationHandle<IList<SkinData>> _loadHandle, List<SkinData> _skinsData)
        {
            _storeSkins = _skinsData;
            _skinDataLoadHandle = _loadHandle;
        }
    }
}
