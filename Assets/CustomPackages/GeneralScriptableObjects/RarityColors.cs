using UnityEngine;

namespace GeneralScriptableObjects
{
    [CreateAssetMenu(fileName = "RarityColor", menuName = "ScriptableObjects/Settings/RarityColor", order = 0)]
    public class RarityColors : ScriptableObject
    {
        [SerializeField] 
        private Color[] _rarityColors;

        public Color[] Values => _rarityColors;
    }
}