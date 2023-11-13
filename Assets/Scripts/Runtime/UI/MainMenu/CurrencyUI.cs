using System;
using System.Collections;
using Runtime.Enums;
using SceneManagementSystem.Scripts;
using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _textComponent;

        [SerializeField] 
        private ECurrencyType _currencyType;

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            while (GameManager.Instance == null || GameManager.Instance.DataLoaded == false)
            {
                yield return null;
            }

            GameManager.Instance.PlayerDataContainer.Currencies.BalanceChanged += UpdateCurrency;
            
            UpdateCurrency();
        }

        private void OnDestroy()
        {
            GameManager.Instance.PlayerDataContainer.Currencies.BalanceChanged -= UpdateCurrency;
        }

        private void UpdateCurrency()
        {
            switch (_currencyType)
            {
                case ECurrencyType.SoftCurrency:
                    _textComponent.text =
                        GameManager.Instance.PlayerDataContainer.Currencies.SoftCurrencyBalance.ToString();
                    break;
                case ECurrencyType.HardCurrency:
                    _textComponent.text =
                        GameManager.Instance.PlayerDataContainer.Currencies.HardCurrencyBalance.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
