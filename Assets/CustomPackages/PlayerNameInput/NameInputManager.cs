using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerNameInput
{
    public class NameInputManager : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _nameInputGo;

        [SerializeField] 
        private TMP_InputField _inputField;
        
        [SerializeField] 
        private UnityEvent _onShowNameInput;
        
        [SerializeField] 
        private UnityEvent _onHideNameInput;
        
        public void CheckName()
        {
            var playerName = PlayerNameDataManager.LoadPlayerName();
            if (playerName == String.Empty)
            {
                ShowNameInputUI();
            }
            else
            {
                HideNameInputUI();
            }
        }

        private void HideNameInputUI()
        {
            _onHideNameInput?.Invoke();
            _nameInputGo.SetActive(false);
        }

        private void ShowNameInputUI()
        {
            _onShowNameInput?.Invoke();
            _nameInputGo.SetActive(true);
        }

        public void OnConfirmButtonClicked()
        {
            PlayerNameDataManager.SavePlayerName(_inputField.text);
            HideNameInputUI();
        }
    }
}