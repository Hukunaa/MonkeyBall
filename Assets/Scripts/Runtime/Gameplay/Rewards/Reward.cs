namespace Gameplay.Rewards
{
    public struct Reward
    {
        public int currencyReward;
        public int battlePassXP;

        public Reward(int _currencyReward, int _battlePassXP)
        {
            currencyReward = _currencyReward;
            battlePassXP = _battlePassXP;
        }
    }
}