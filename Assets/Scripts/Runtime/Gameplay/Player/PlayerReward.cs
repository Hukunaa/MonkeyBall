using System.Linq;
using Gameplay.Rewards;
using Runtime.Enums;
using SceneManagementSystem.Scripts;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Player
{
    public class PlayerReward : MonoBehaviour
    {
        [SerializeField] 
        private PlayersRankContainer _playersRankContainer;

        [SerializeField] 
        private UnityEvent<Reward> _onRewardObtained;
        
        public void GetRankReward()
        {
            var playerRank = _playersRankContainer.Ranking.FirstOrDefault(x => x.Value.IsPlayer).Key;
            
            var reward = RewardManager.GetReward(playerRank);
            GameManager.Instance.PlayerDataContainer.Currencies.AddBalance(ECurrencyType.SoftCurrency, reward.currencyReward);
            GameManager.Instance.PlayerDataContainer.PlayerBattlePassXp.AddXp(reward.battlePassXP);
            _onRewardObtained?.Invoke(reward);
        }
    }
}
