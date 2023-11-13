using System;
using UnityEngine;

namespace UIPackage.Scripts
{
    public abstract class StoreItem : MonoBehaviour
    {
        private bool _isSelected;

        [SerializeField]
        private Canvas _itemCanvas;

        [SerializeField] 
        private GameObject _selectedUI;

        public event Action<StoreItem> OnStoreItemClicked;
        
        protected int _price;

        private void Start()
        {
            DeselectItem();
        }

        public void OnItemButtonClicked()
        {
            OnStoreItemClicked?.Invoke(this);
        }
        
        public void SelectItem()
        {
            _isSelected = true;
            _selectedUI.SetActive(true);
            _itemCanvas.overrideSorting = true;
            _itemCanvas.sortingOrder = 1;
        }

        public void DeselectItem()
        {
            _isSelected = false;
            _selectedUI.SetActive(false);
            _itemCanvas.overrideSorting = false;
        }
        
        public abstract void ShowItemInfo();
        
        public int Price => _price;
    }
}