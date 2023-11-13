using System;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SpeedBonusScoringTable", menuName = "ScriptableObjects/Settings/SpeedBonusScoringTable", order = 0)]
    public class SpeedBonusScoringTable : ScriptableObject
    {
        [SerializeField]
        private int[] _scoreEntry;

        public int[] ScoreEntry => _scoreEntry;
    }
}