using System.Threading.Tasks;
using DataContainers;
using PlayerNameInput;
using Runtime.DataContainers.Player;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Runtime.ScriptableObjects.DataContainers
{
    [CreateAssetMenu(fileName = "PlayerDataManager", menuName = "ScriptableObjects/DataContainer/PlayerData/PlayerDataManager", order = 0)]
    public class PlayerDataContainer : ScriptableObject
    {
        [SerializeField] 
        private string _playerName;
        
        [SerializeField]
        private PlayerScore _playerScore;

        [SerializeField]
        private PlayerCurrencies _playerCurrencies;

        [SerializeField]
        private PlayerSkinInventory _playerSkinsInventory;

        [SerializeField]
        private PlayerBattlePassXp _playerBattlePassXP;
        
        public UnityAction OnPlayerDataLoaded;

        public async Task LoadPlayerData()
        {
            _playerName = PlayerNameDataManager.LoadPlayerName();
            _playerCurrencies = new PlayerCurrencies();
            _playerCurrencies.LoadBalance();
            _playerScore = new PlayerScore();
            _playerScore.LoadScore();
            _playerBattlePassXP = new PlayerBattlePassXp();
            _playerBattlePassXP.LoadBattlePassXp();
            _playerSkinsInventory = new PlayerSkinInventory();
            await _playerSkinsInventory.LoadSkins();
            
            OnPlayerDataLoaded?.Invoke();
        }

        public PlayerScore PlayerScore { get => _playerScore; }
        public PlayerCurrencies Currencies { get => _playerCurrencies; }
        public PlayerSkinInventory PlayerSkinsInventory => _playerSkinsInventory;
        public string PlayerName { get => _playerName; }
        public PlayerBattlePassXp PlayerBattlePassXp { get => _playerBattlePassXP; }

        public UnityAction OnPlayerNameChanged;
    }
}