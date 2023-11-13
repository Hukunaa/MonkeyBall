using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utilities;

namespace Gameplay.Rewards
{
    public static class RewardManager
    {
        private static Dictionary<int, Reward> _rewardDatabase =
            new Dictionary<int, Reward>();
        
        private const string RewardTablePath = "RewardData/RewardTable";

        private static bool _dataBaseLoaded;

        public static Reward GetReward(int _rank)
        {
            if (_dataBaseLoaded == false)
            {
                LoadRewardTable();
            }
            
            return _rewardDatabase[_rank];
        }
        
        private static void LoadRewardTable()
        {
            _rewardDatabase.Clear();

            var csv = Resources.Load<TextAsset>(RewardTablePath);
            List<Dictionary<string, object>> data = CSVReader.Read(csv);
            for (int i = 0; i < data.Count; i++)
            {
                int rank = int.Parse(data[i]["rank"].ToString(), NumberStyles.Integer);
                int softCurrency = int.Parse(data[i]["softCurrency"].ToString(), NumberStyles.Integer);
                int battlePassXP = int.Parse(data[i]["battlePassXP"].ToString(), NumberStyles.Integer);
                AddItem(rank, softCurrency, battlePassXP);
            }

            _dataBaseLoaded = true;
            Debug.Log($"Reward table loaded: {_rewardDatabase.Count} entries");
        }
        
        private static void AddItem(int _rank, int _softCurrencyReward, int _battlePassReward)
        {
            _rewardDatabase[_rank] = new Reward(_softCurrencyReward, _battlePassReward);
        }
    }
}