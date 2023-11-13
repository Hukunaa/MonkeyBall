using System;
using System.Collections.Generic;
using System.Globalization;
using Runtime.Enums;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.DataContainer
{
    [CreateAssetMenu(fileName = "PriceTable", menuName = "ScriptableObjects/DataContainer/PriceTable", order = 0)]
    public class PriceTableSO : ScriptableObject
    {
        [SerializeField] 
        private TextAsset _priceTable;

        private Dictionary<string, Tuple<ECurrencyType, int>> _skinPriceDataBase =
            new Dictionary<string, Tuple<ECurrencyType, int>>();
        
        public void LoadPriceTable()
        {
            _skinPriceDataBase.Clear();

            List<Dictionary<string, object>> data = CSVReader.Read(_priceTable);
            for (int i = 0; i < data.Count; i++)
            {
                string name = data[i]["key"].ToString();
                ECurrencyType currencyType = (ECurrencyType)int.Parse(data[i]["currencyType"].ToString(), NumberStyles.Integer);
                int price = int.Parse(data[i]["value"].ToString(), NumberStyles.Integer);
                AddItem(name, currencyType, price);
            }
            
            Debug.Log($"Skin price table loaded: {_skinPriceDataBase.Count} entries");
        }

        public Tuple<ECurrencyType, int> GetItemPrice(string _key)
        {
            return _skinPriceDataBase[_key];
        }
        
        private void AddItem(string _name, ECurrencyType _currencyType, int _price)
        {
            _skinPriceDataBase[_name] = new Tuple<ECurrencyType, int>(_currencyType, _price);
        }
    }
}