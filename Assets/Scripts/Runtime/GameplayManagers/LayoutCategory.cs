using System;
using ScriptableObjects.Settings;
using UnityEngine;


namespace GameplayManagers
{
    [Serializable]
    public class LayoutCategory
    {
        [SerializeField]
        private Vector2Int _minMaxRange;

        [SerializeField] 
        private LayoutSO[] _layoutSo;

        public Vector2Int MinMaxRange => _minMaxRange;

        public LayoutSO[] LayoutSO => _layoutSo;
    }
}