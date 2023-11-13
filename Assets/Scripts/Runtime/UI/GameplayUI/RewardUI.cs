using Gameplay.Rewards;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _currencyRewardTmp;
        
        [SerializeField] 
        private TMP_Text _battlePassRewardTmp;
        
        public void UpdateRewardUI(Reward _reward)
        {
            _currencyRewardTmp.text = _reward.currencyReward.ToString();
            _battlePassRewardTmp.text = _reward.battlePassXP.ToString();
        }
    }
}
