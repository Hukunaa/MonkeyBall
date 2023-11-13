using System;

namespace Utilities
{
    [Serializable]
    public class PriceItem
    {
        public string skinName;
        public int price;

        public PriceItem(string _name, int _price)
        {
            skinName = _name;
            price = _price;
        }
    }
}