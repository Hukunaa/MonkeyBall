using System;
using System.Collections.Generic;
using ScriptableObjects.Settings;
using ScriptableObjects.DataContainer;
using ScriptableObjects.EventChannels;
using UIPackage.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.MainMenu.StoreUI
{
    public class SkinItem : StoreItem
    {
        [SerializeField] 
        private Image _skinIcon;
        [SerializeField]
        private Image _skinBackground;
        [SerializeField]
        private RarityColors _rarityColors;
        [SerializeField] 
        private AssetReferenceT<SkinDataEventChannel> _onShowSkinInfoEventChannel;

        private SkinData _skinData;

        
        private SkinDataEventChannel _showSkinInfoEventChannel;

        public UnityAction onShowItemInfo;
        private AsyncOperationHandle<SkinDataEventChannel> _showSkinInfoEventChannelLoadHandle;

        private void Awake()
        {
            _showSkinInfoEventChannelLoadHandle = _onShowSkinInfoEventChannel.LoadAssetAsync<SkinDataEventChannel>();
            _showSkinInfoEventChannelLoadHandle.Completed += _handle =>
            {
                _showSkinInfoEventChannel = _handle.Result;
            };
        }

        private void OnDestroy()
        {
            Addressables.Release(_showSkinInfoEventChannelLoadHandle);
        }

        public void Initialize(SkinData _skinData)
        {
            this._skinData = _skinData;

            _skinIcon.sprite = _skinData.SkinIcon;

            switch(_skinData.Rarity)
            {
                case Enums.ERarityType.COMMON:
                    _skinBackground.color = _rarityColors.Colors[0];
                    break;
                case Enums.ERarityType.UNCOMMON:
                    _skinBackground.color = _rarityColors.Colors[1];
                    break;
                case Enums.ERarityType.RARE:
                    _skinBackground.color = _rarityColors.Colors[2];
                    break;
                case Enums.ERarityType.EPIC:
                    _skinBackground.color = _rarityColors.Colors[3];
                    break;
                case Enums.ERarityType.LEGENDARY:
                    _skinBackground.color = _rarityColors.Colors[4];
                    break;

            }
        }
        
        public override void ShowItemInfo()
        {
            _showSkinInfoEventChannel.RaiseEvent(_skinData);
            onShowItemInfo?.Invoke();
        }

        public SkinData SkinData => _skinData;
    }
}
