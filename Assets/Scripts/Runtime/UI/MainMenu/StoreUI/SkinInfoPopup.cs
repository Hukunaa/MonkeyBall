using SceneManagementSystem.Scripts;
using ScriptableObjects.DataContainer;
using ScriptableObjects.EventChannels;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.MainMenu.StoreUI
{
    public class SkinInfoPopup : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _skinNameTmp;
        
        [SerializeField] 
        private Image _skinIcon;

        [SerializeField] 
        private PurchaseButton _purchaseButton;
        
        [SerializeField] 
        private AssetReferenceT<PriceTableSO>  _priceTableAssetRef;

        private AsyncOperationHandle<PriceTableSO> _priceTableLoadHandle;
        
        [SerializeField] 
        private PriceTableSO _priceTable;
        
        [Header("Listening to")]
        [SerializeField] 
        private AssetReferenceT<SkinDataEventChannel> _onShowSkinInfoEventChannelAssetRef;
        
        [Header("Broadcast on")]
        [SerializeField] 
        private AssetReferenceT<SkinDataEventChannel> _onItemBoughtEventChannelAssetRef;

        [SerializeField] 
        private UnityEvent _onPopupInitialized;
        
        [SerializeField] 
        private UnityEvent _onItemPurchased;
        
        private SkinData _currentSkinData;
        
        private AsyncOperationHandle<SkinDataEventChannel> _showSkinEventChannelLoadHandle;
        private SkinDataEventChannel _onShowSkinInfoEventChannel;
        
        private AsyncOperationHandle<SkinDataEventChannel> _onItemBoughtEventChannelLoadHandle;
        private SkinDataEventChannel _onItemBoughtEventChannel;

        protected void Awake()
        {
            _showSkinEventChannelLoadHandle = _onShowSkinInfoEventChannelAssetRef.LoadAssetAsync<SkinDataEventChannel>();
            _showSkinEventChannelLoadHandle.Completed += _handle =>
            {
                _onShowSkinInfoEventChannel = _handle.Result;
                _onShowSkinInfoEventChannel.onEventRaised += ShowSkinInfoPopup;
            };

            _onItemBoughtEventChannelLoadHandle = _onItemBoughtEventChannelAssetRef.LoadAssetAsync<SkinDataEventChannel>();
            _onItemBoughtEventChannelLoadHandle.Completed += _handle =>
            {
                _onItemBoughtEventChannel = _handle.Result;
            };

            _priceTableLoadHandle = _priceTableAssetRef.LoadAssetAsync<PriceTableSO>();
            _priceTableLoadHandle.Completed += _handle =>
            {
                _priceTable = _handle.Result;
            };
        }

        private void OnDestroy()
        {
            _onShowSkinInfoEventChannel.onEventRaised -= ShowSkinInfoPopup;
            Addressables.Release(_showSkinEventChannelLoadHandle);
            Addressables.Release(_onItemBoughtEventChannelLoadHandle);
            Addressables.Release(_priceTableLoadHandle);
        }

        private void OnEnable()
        {
            _purchaseButton.onPurchaseButtonClicked += PurchaseItem;
        }

        private void OnDisable()
        {
            _purchaseButton.onPurchaseButtonClicked -= PurchaseItem;
        }

        private void ShowSkinInfoPopup(SkinData _skinData)
        {
            _currentSkinData = _skinData;
            _skinNameTmp.text = _skinData.SkinName;
            _skinIcon.sprite = _skinData.SkinIcon;
            var priceInfo = _priceTable.GetItemPrice(_skinData.name);
            _purchaseButton.UpdatePrice(priceInfo.Item1, priceInfo.Item2);
            _onPopupInitialized?.Invoke();
        }
        
        private void PurchaseItem()
        {
            GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory.AddSkin(_currentSkinData);
            _onItemBoughtEventChannel.RaiseEvent(_currentSkinData);
            var priceInfo = _priceTable.GetItemPrice(_currentSkinData.name);
            GameManager.Instance.PlayerDataContainer.Currencies.Pay(priceInfo.Item1, priceInfo.Item2);
            _onItemPurchased?.Invoke();
        }
    }
}