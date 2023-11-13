using System.Collections;
using Runtime.ScriptableObjects.DataContainers;
using SceneManagementSystem.Scripts;
using TMPro;
using UnityEngine;

namespace Runtime.UI.MainMenuUI
{
    public class MainMenuHeader : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _softCurrencyBalance;
        [SerializeField] private TMP_Text _hardCurrencyBalance;
        [SerializeField] private TMP_Text _highScore;
        
        private PlayerDataContainer _playerDataContainer;

        private void Start()
        {
            StartCoroutine(InitializeCoroutine());
        }
        
        private IEnumerator InitializeCoroutine()
        {
            while (GameManager.Instance == null || !GameManager.Instance.DataLoaded)
            {
                yield return null;
            }

            _playerDataContainer = GameManager.Instance.PlayerDataContainer;
            
            Initialize();
        }

        private void Initialize()
        {
            Subscribe();
            UpdatePlayerInfo();
            UpdatePlayerName();
        }

        private void Subscribe()
        {
            _playerDataContainer.Currencies.BalanceChanged += UpdatePlayerInfo;
            _playerDataContainer.PlayerScore.ScoreChanged += UpdatePlayerInfo;
            _playerDataContainer.OnPlayerNameChanged += UpdatePlayerName;
        }
        private void Unsubscribe()
        {
            _playerDataContainer.Currencies.BalanceChanged -= UpdatePlayerInfo;
            _playerDataContainer.PlayerScore.ScoreChanged -= UpdatePlayerInfo;
            _playerDataContainer.OnPlayerNameChanged -= UpdatePlayerName;
        }

        private void UpdatePlayerInfo()
        {
            _softCurrencyBalance.text = _playerDataContainer.Currencies.SoftCurrencyBalance.ToString();
            _hardCurrencyBalance.text = _playerDataContainer.Currencies.HardCurrencyBalance.ToString();
            _highScore.text = _playerDataContainer.PlayerScore.Score.ToString();
        }

        private void UpdatePlayerName()
        {
            _playerName.text = _playerDataContainer.PlayerName;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}