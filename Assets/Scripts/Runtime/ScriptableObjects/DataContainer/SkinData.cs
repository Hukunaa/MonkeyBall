using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScriptableObjects.DataContainer
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/DataContainer/SkinData", order = 0)]
    public class SkinData : ScriptableObject
    {
        [SerializeField] 
        private string _skinName;
        
        [SerializeField]
        private ERarityType _rarityType;

        [SerializeField] 
        private ESkinType _skinType;
        
        [SerializeField] 
        private Object _meshAssetRef;

        [SerializeField]
        private Object[] _skinColorAssetRefs;

        [SerializeField]
        private AssetReference _ballSkinRef;

        [SerializeField]
        private AssetReference _gliderSkinRef;

        [SerializeField] 
        private Sprite _skinIcon;

        [SerializeField]
        private Material _customMaterial;

        public string SkinName => _skinName;
        public ERarityType Rarity => _rarityType;
        public ESkinType SkinType => _skinType;
        public Object MeshAssetRef => _meshAssetRef;
        public Sprite SkinIcon => _skinIcon;
        public AssetReference BallSkinRef { get => _ballSkinRef; }
        public AssetReference GliderSkinRef { get => _gliderSkinRef; }
        public Object[] SkinColorAssetRefs { get => _skinColorAssetRefs; }
        public Material CustomMaterial { get => _customMaterial; }
    }
}