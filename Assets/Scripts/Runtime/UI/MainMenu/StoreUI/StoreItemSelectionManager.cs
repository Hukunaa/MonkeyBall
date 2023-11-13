using UIPackage.Scripts;
using UnityEngine;

namespace UI.MainMenu.StoreUI
{
    public class StoreItemSelectionManager : MonoBehaviour
    {
        [SerializeField]
        private StoreItem[] _storeItems;

        private StoreItem _selectedStoreItem;

        public void Initialize()
        {
            _storeItems = GetComponentsInChildren<StoreItem>();

            foreach (var storeItem in _storeItems)
            {
                storeItem.OnStoreItemClicked += OnItemButtonClicked;
            }
        }
    
        private void OnItemButtonClicked(StoreItem _clickedItem)
        {
            if (_selectedStoreItem == null)
            {
                SelectNewItem(_clickedItem);
                return;
            }

            if (_clickedItem == _selectedStoreItem)
            {
                ClearSelectedItem();
                return;
            }
        
            ClearSelectedItem();
            SelectNewItem(_clickedItem);
        }

        public void ClearSelectedItem()
        {
            _selectedStoreItem.DeselectItem();
            _selectedStoreItem = null;
        }

        private void SelectNewItem(StoreItem _newItem)
        {
            _selectedStoreItem = _newItem;
            _selectedStoreItem.SelectItem();
        }
    }
}
