using System;
using Gameplay.Rewards;
using GeneralScriptableObjects;
using UnityEngine;

namespace Gameplay.Character
{
    public class PlayerScore : MonoBehaviour, IScore
    {
        private int _currentScore;
        public event Action<int, int, int, int> onScoreCalculated;
        public event Action onScoreChanged;

        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _targetLayerMask;
        
        [SerializeField] 
        private IntVariable _meterPerPointRatio;

        private PlayerDistanceManager _playerDistanceManager;

        private void Awake()
        {
            _playerDistanceManager = GetComponent<PlayerDistanceManager>();
        }

        public void CalculateScore()
        {
            int distance = _playerDistanceManager.CalculateDistance();
            int points = distance / _meterPerPointRatio.Value;
            
            //Get multiplier value
            int multiplier = 1;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _targetLayerMask))
            {
                multiplier = hit.collider.GetComponent<TargetZone>().GetModifierAtPosition(transform.position);
            }

            int finalScore = multiplier * points;
            
            onScoreCalculated?.Invoke(distance, points, multiplier, finalScore);
            AddScore(finalScore);
        }

        public void ResetScore()
        {
            _currentScore = 0;
            onScoreChanged?.Invoke();
        }
        
        public void AddScore(int _score)
        {
            _currentScore += _score;
            onScoreChanged?.Invoke();
        }
        
        public void RemoveScore(int _score)
        {
            _currentScore -= _score;
            onScoreChanged?.Invoke();
        }

        public int CurrentScore => _currentScore;
    }
}
