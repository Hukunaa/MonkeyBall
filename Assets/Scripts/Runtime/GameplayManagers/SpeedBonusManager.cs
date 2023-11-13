using Gameplay.Character;
using Gameplay.Rewards;
using GeneralScriptableObjects;
using ScriptableObjects.Settings;
using UnityEngine;

namespace GameplayManagers
{
    public class SpeedBonusManager : MonoBehaviour
    {
        [SerializeField] 
        private SpeedBonusScoringTable _speedBonusTable;

        [SerializeField] 
        private IntVariable _playerTurnCompleteCount;
        
        private PlayerTurnManager _playerTurnManager;

        private void Awake()
        {
            _playerTurnManager = FindObjectOfType<PlayerTurnManager>();
            _playerTurnManager.onPlayerTurnComplete += AwardSpeedBonus;
        }

        private void OnDestroy()
        {
            _playerTurnManager.onPlayerTurnComplete -= AwardSpeedBonus;
        }

        private void AwardSpeedBonus(Player _player)
        {
            if (_player == null) return;
            _player.Score.AddScore(_speedBonusTable.ScoreEntry[_playerTurnCompleteCount.Value - 1]);
        }
    }
}
