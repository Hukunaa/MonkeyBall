using System;
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

namespace DataContainers
{
    [Serializable]
    public class PlayerSkinInventory
    {
        [SerializeField]
        private List<SkinData> _headSkins = new List<SkinData>();
       
        [SerializeField]
        private List<SkinData> _bodySkins = new List<SkinData>();
        
        [SerializeField]
        private List<SkinData> _ballSkins = new List<SkinData>();
        
        [SerializeField]
        private List<SkinData> _gliderSkins = new List<SkinData>();

        public UnityAction<SkinData> _onSkinAdded;
        public UnityAction<SkinData> _onSkinRemoved;

        private AsyncOperationHandle<IList<SkinData>> _skinsLoadHandle;
        
        private const string SkinsInventoryPath = "PlayerData/PlayerSkinsInventory.json";

        public async Task LoadSkins()
        {
            var skinsData = DataLoader.LoadSkins(DataLoader.PersistentPlayerSkinsInventoryDataPath);

            var skinKeys =  skinsData.Select(x => $"{AddressableLoadingUtilities.SkinsAddressablePath}{x}.asset").AsEnumerable();

            var skins = await AddressableLoadingUtilities.LoadAddressableByKeysAsync<SkinData>(skinKeys);
            _skinsLoadHandle = skins.Item2;

            foreach (var skin in skins.Item1)
            {
                AddSkinInternal(skin);
            }
        }

        public void UnloadSkins()
        {
            Addressables.Release(_skinsLoadHandle);
        }
        
        private void SaveSkins()
        {
            var headSkinNames= _headSkins.Select(x => x.name).ToList();
            var bodySkinNames= _bodySkins.Select(x => x.name).ToList();
            var ballSkinNames= _ballSkins.Select(x => x.name).ToList();
            var gliderSkinNames= _gliderSkins.Select(x => x.name).ToList();
            
            var combinedArray = headSkinNames.Concat(bodySkinNames).Concat(ballSkinNames).Concat(gliderSkinNames)
                .ToArray();
            
            DataLoader.SaveSkins(DataLoader.PersistentPlayerSkinsInventoryDataPath, combinedArray);
        }

        private void AddSkinInternal(SkinData _skin)
        {
            switch (_skin.SkinType)
            {
                case ESkinType.HEAD:
                    _headSkins.Add(_skin);
                    break;
                case ESkinType.BODY:
                    _bodySkins.Add(_skin);
                    break;
                case ESkinType.BALL:
                    _ballSkins.Add(_skin);
                    break;
                case ESkinType.GLIDER:
                    _gliderSkins.Add(_skin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddSkin(SkinData _skin)
        {
            if (ContainSkins(_skin))
            {
                Debug.Log($"There is already {_skin.SkinName} in Skin Inventory. Cannot add it.");
                return;
            }

            AddSkinInternal(_skin);
            _onSkinAdded?.Invoke(_skin);
            SaveSkins();
        }
        
        private void RemoveSkinInternal(SkinData _skin)
        {
            switch (_skin.SkinType)
            {
                case ESkinType.HEAD:
                    _headSkins.Remove(_skin);
                    break;
                case ESkinType.BODY:
                    _bodySkins.Remove(_skin);
                    break;
                case ESkinType.BALL:
                    _ballSkins.Remove(_skin);
                    break;
                case ESkinType.GLIDER:
                    _gliderSkins.Remove(_skin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveSkin(SkinData _skin)
        {
            if (ContainSkins(_skin) == false)
            {
                Debug.Log($"Could not find {_skin.SkinName} in Skin Inventory. Cannot remove it.");
                return;
            }
            
            RemoveSkinInternal(_skin);
            _onSkinRemoved?.Invoke(_skin);
            SaveSkins();
        }

        private bool ContainSkins(SkinData _skin)
        {
            switch (_skin.SkinType)
            {
                case ESkinType.HEAD:
                    return _headSkins.Contains(_skin);
                case ESkinType.BODY:
                    return _bodySkins.Contains(_skin);
                case ESkinType.BALL:
                    return _ballSkins.Contains(_skin);
                case ESkinType.GLIDER:
                    return _gliderSkins.Contains(_skin);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<SkinData> HeadSkins => _headSkins;

        public List<SkinData> BodySkins => _bodySkins;

        public List<SkinData> BallSkins => _ballSkins;

        public List<SkinData> GliderSkins => _gliderSkins;
    }
}