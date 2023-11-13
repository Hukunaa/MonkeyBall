using ScriptableObjects.DataContainers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.DataContainers
{
    [CreateAssetMenu(fileName = "RewardItem", menuName = "ScriptableObjects/Rewards/BattlePassManager", order = 0)]
    public class BattlePassRewardManager : ScriptableObject
    {
        [SerializeField]
        private List<RewardItem> _rewards;


        public void Initialize()
        {
            bool[] isUsedData = DataLoader.LoadPlayerRewardsState();
            if (isUsedData.Length != _rewards.Count)
                DataLoader.SavePlayerRewardsState(_rewards.Select(p => p.IsUsed).ToArray());

            isUsedData = DataLoader.LoadPlayerRewardsState();

            for (int i = 0; i < _rewards.Count; ++i)
                if (isUsedData[i])
                    _rewards[i].Complete();
        }
        public List<RewardItem> Rewards { get => _rewards; }
    }
}
