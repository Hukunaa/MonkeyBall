using System;
using Runtime.Enums;
using SceneManagementSystem.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.MainMenu.StoreUI
{
    [RequireComponent(typeof(Button))]
    public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _priceText;

        [SerializeField] 
        private Image _currencyIcon;

        [SerializeField] private Sprite _softCurrencySprite;
        [SerializeField] private Sprite _hardCurrencySprite;

        [SerializeField]
        private Color _disableTextColor;

        public UnityAction onPurchaseButtonClicked;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void UpdatePrice(ECurrencyType _currencyType, int _price)
        {
            _priceText.text = _price.ToString();

            switch (_currencyType)
            {
                case ECurrencyType.SoftCurrency:
                    _currencyIcon.sprite = _softCurrencySprite;
                    break;
                case ECurrencyType.HardCurrency:
                    _currencyIcon.sprite = _hardCurrencySprite;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currencyType), _currencyType, null);
            }
            _button.interactable = (GameManager.Instance.PlayerDataContainer.Currencies.CanPay(_currencyType, _price));

            _priceText.color = GameManager.Instance.PlayerDataContainer.Currencies.CanPay(_currencyType, _price) ? Color.white : _disableTextColor;
        }

        public void Purchase()
        {
            onPurchaseButtonClicked?.Invoke();
        }
    }
}