using System;
using UnityEngine;

namespace Gameplay.Rewards
{
    [Serializable]
    public class ScoreArea
    {
        [SerializeField] private float _distanceFromCenter;
        [SerializeField] private int _points;

        public float DistanceFromCenter => _distanceFromCenter;

        public float SqrDistanceFromCenter => _distanceFromCenter * _distanceFromCenter;

        public int Points => _points;
    }
}