using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.DataContainers.Player
{
    public class PlayerProgression
    {
        [SerializeField]
        private int _level;
        [SerializeField]
        private int _currentXP;
        [SerializeField]
        private int _currentLevelXPGoal;
        [SerializeField]
        private float _xpGoalIncreaseMultiplier;
        public PlayerProgression(int level, int currentXP, int currentLevelXPGoal, float XpGoalIncreaseMultiplier)
        {
            _level = level;
            _currentLevelXPGoal = currentLevelXPGoal;
            _currentXP = currentXP;
            _xpGoalIncreaseMultiplier = XpGoalIncreaseMultiplier;
        }

        public void LevelUp()
        {
            //Need Server info for implementation
        }

        public int Level { get => _level; set => _level = value; }
        public int CurrentXP { get => _currentXP; }
        public int CurrentLevelXPGoal { get => _currentLevelXPGoal; }
        public float XpGoalIncreaseMultiplier { get => _xpGoalIncreaseMultiplier; }
    }
}
