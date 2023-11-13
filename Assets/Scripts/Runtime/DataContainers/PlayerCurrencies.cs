using Runtime.Enums;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using Utilities;

namespace Runtime.DataContainers.Player
{
    [Serializable]
    public class PlayerCurrencies
    {
        [SerializeField]
        private int _softCurrencyBalance;
        
        [SerializeField]
        private int _hardCurrencyBalance;

        public UnityAction BalanceChanged;
        
        public void LoadBalance()
        {
            int[] currencies = DataLoader.LoadCurrencies();
            if (currencies == null)
                return;

            _softCurrencyBalance = currencies[0];
            _hardCurrencyBalance = currencies[1];
        }

        public void SaveBalance()
        {
            Debug.Log("Saving new balance: " + _softCurrencyBalance + " / " + _hardCurrencyBalance);
            DataLoader.SaveCurrencies(_softCurrencyBalance, _hardCurrencyBalance);
        }

        public void AddBalance(ECurrencyType _currencyType, int _amount)
        {
            Debug.Log($"Adding {_amount} {_currencyType.ToString()} to balance.");
            //Need server side authorization
            switch (_currencyType)
            {
                case ECurrencyType.SoftCurrency:
                    _softCurrencyBalance += _amount;
                    break;
                case ECurrencyType.HardCurrency:
                    _hardCurrencyBalance += _amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currencyType), _currencyType, null);
            }
           
            BalanceUpdated();
        }

        public bool CanPay(ECurrencyType _currencyType, int _amount)
        {
            switch (_currencyType)
            {
                case ECurrencyType.SoftCurrency:
                    
                    if (_softCurrencyBalance >= _amount)
                    {
                        return true;
                    }
                    return false;

                case ECurrencyType.HardCurrency:
                    if (_hardCurrencyBalance >= _amount)
                    {
                        return true;
                    }
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currencyType), _currencyType, null);
            }
        }
        public bool Pay(ECurrencyType _currencyType, int _amount)
        {
            //Need server side authorization
            switch (_currencyType)
            {
                case ECurrencyType.SoftCurrency:
                    if (_softCurrencyBalance > 0)
                    {
                        if (_softCurrencyBalance >= _amount)
                        {
                            _softCurrencyBalance -= _amount;
                            BalanceUpdated();
                            return true;
                        }
                        return false;
                    }
                    return false;
                case ECurrencyType.HardCurrency:
                    if (_hardCurrencyBalance > 0)
                    {
                        if (_hardCurrencyBalance >= _amount)
                        {
                            _hardCurrencyBalance -= _amount;
                            BalanceUpdated();
                            return true;
                        }
                        return false;
                    }
                    
                    return false;
                    

                default:
                    throw new ArgumentOutOfRangeException(nameof(_currencyType), _currencyType, null);
            }
        }

        private void BalanceUpdated()
        {
            SaveBalance();
            BalanceChanged?.Invoke();
        }

        public int SoftCurrencyBalance { get => _softCurrencyBalance; }
        public int HardCurrencyBalance { get => _hardCurrencyBalance; }
    }
}

