using System;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "KnockOutSettings", menuName = "ScriptableObjects/Settings/KnockOutSettings", order = 0)]
    public class KnockOutSettings : ScriptableObject
    {
        [SerializeField] 
        private KnockOutRoundSetting[] _knockOutRoundSettings;
        
        public int GetRoundKnockoutCount(int _round)
        {
            var roundKnockOutSettings = KnockOutRoundSettings.FirstOrDefault(x => x.round == _round);
            if (roundKnockOutSettings != null) return roundKnockOutSettings.knockOutAmount;
            Debug.LogError($"Can't find a Knockout Setting for round {_round}");
            return -1;

        }

        public KnockOutRoundSetting[] KnockOutRoundSettings => _knockOutRoundSettings;
    }

    [Serializable]
    public class KnockOutRoundSetting
    {
        public int round;
        public int knockOutAmount;
    }
}