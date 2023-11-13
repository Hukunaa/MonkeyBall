using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "LayoutSO", menuName = "ScriptableObjects/Settings/LayoutSO", order = 0)]
    public class LayoutSO : ScriptableObject
    {
        [SerializeField][Range(1,10)]
        private int _attempts = 1;
        
        [SerializeField] 
        private AssetReference _sceneAssetRef;

        [SerializeField] 
        private Sprite _layoutSprite;

        [SerializeField] 
        private string _layoutName;
        
        public int Attempts => _attempts;

        public AssetReference SceneAssetRef => _sceneAssetRef;

        public Sprite LayoutSprite => _layoutSprite;

        public string LayoutName => _layoutName;
    }
}