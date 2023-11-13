using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Runtime.DataContainers.Player
{
    [Serializable]
    public class PlayerScore
    {
        [SerializeField]
        private int _score;
        [SerializeField]
        private int _wins;

        public UnityAction ScoreChanged;
        public UnityAction WinsChanged;
        
        public void LoadScore()
        {
            int[] data = DataLoader.LoadScore();
            _score = data[0];
            _wins = data[1];
        }
        
        public void SaveScore()
        {
            Debug.Log($"Saving Score: {_score.ToString()}");
            DataLoader.SaveScore(_score, _wins);
        }

        public void UpdateScore(int _newScore)
        {
            //Need server side authorization
            if (_newScore <= _score) return;
            _score = _newScore;
            ScoreChanged?.Invoke();
            SaveScore();
        }
        public void UpdateWins(int _newWins)
        {
            //Need server side authorization
            if (_newWins <= _wins) return;
            _wins = _newWins;
            WinsChanged?.Invoke();
            SaveScore();
        }

        public int Score { get => _score; }
        public int Wins { get => _wins; }
    }
}

