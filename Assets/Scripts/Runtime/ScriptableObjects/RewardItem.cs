using ScriptableObjects.DataContainer;
using UnityEngine;

namespace ScriptableObjects
{
    public enum REWARD_TYPE
    {
        COINS,
        SKIN,
        NONE
    }

    [CreateAssetMenu(fileName = "RewardItem", menuName = "ScriptableObjects/Rewards/UIRewardItem", order = 0)]
    public class RewardItem : ScriptableObject
    {
        [SerializeField]
        private REWARD_TYPE _rewardType;
        [SerializeField]
        private string _title;
        [SerializeField]
        private string _description;
        [SerializeField]
        private int _xpRequired;
        [SerializeField]
        private Sprite _rewardSprite;
        //Needs to be updated to support any item later when the reward system is done
        //For now, we only give coins
        [SerializeField]
        private int _coinsReward;
        [SerializeField] 
        private SkinData _skinReward;
        [SerializeField]
        private bool _isUsed;

        public void Complete()
        {
            _isUsed = true;
        }

        public int CoinsReward { get => _coinsReward;  }

        public SkinData SkinReward => _skinReward;

        public string Title { get => _title; }
        public string Description { get => _description; }
        public Sprite RewardSprite { get => _rewardSprite; }
        public REWARD_TYPE RewardType { get => _rewardType; }
        public int XPRequired { get => _xpRequired; }
        public bool IsUsed { get => _isUsed; }
    }
}
