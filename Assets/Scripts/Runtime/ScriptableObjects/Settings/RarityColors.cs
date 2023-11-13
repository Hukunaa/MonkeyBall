using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "RarityColorSettings", menuName = "ScriptableObjects/Settings/RarityColorSettings", order = 0)]
    public class RarityColors : ScriptableObject
    {
        [SerializeField]
        private List<Color> _colors;

        public List<Color> Colors { get => _colors; }
    }
}
