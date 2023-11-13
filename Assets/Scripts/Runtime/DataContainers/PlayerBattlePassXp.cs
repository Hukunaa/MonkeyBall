using System;
using UnityEngine;
using Utilities;

namespace DataContainers
{
    [Serializable]
    public class PlayerBattlePassXp
    {
        private int _battlePassXp;

        public void LoadBattlePassXp()
        {
            _battlePassXp = DataLoader.LoadPlayerBattlePassXp();
        }

        public void AddXp(int _xp)
        {
            Debug.Log($"Adding {_xp} BattlePass Xp.");
            _battlePassXp += _xp;
            SaveBattlePassXp();
        }

        private void SaveBattlePassXp()
        {
            Debug.Log($"Saving BattlePass Xp: {_battlePassXp}");
            DataLoader.SavePlayerBattlePassXp(_battlePassXp);
        }

        public int BattlePassXp => _battlePassXp;
    }
}