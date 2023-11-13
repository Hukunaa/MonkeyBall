using GameplayManagers;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "LayoutSettings", menuName = "ScriptableObjects/Settings/LayoutSettings", order = 0)]
    public class LayoutSettings : ScriptableObject
    {
        [SerializeField] 
        private LayoutCategory[] _layoutCategories;

        public LayoutCategory[] LayoutCategories => _layoutCategories;
    }
}