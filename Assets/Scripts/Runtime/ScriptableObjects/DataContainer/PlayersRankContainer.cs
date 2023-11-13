using System.Collections.Generic;
using Gameplay.Character;
using UnityEngine;

namespace ScriptableObjects.DataContainer
{
    [CreateAssetMenu(fileName = "PlayersRankContainer", menuName = "ScriptableObjects/DataContainer/PlayersRankContainer", order = 0)]
    public class PlayersRankContainer : ScriptableObject
    {
        private Dictionary<int, Player> _ranking = new Dictionary<int, Player>();

        private int _rankCount;

        public void Initialize(int _playerNumber)
        {
            _rankCount = _playerNumber;
            _ranking = new Dictionary<int, Player>();
            Debug.Log($"Initialized Ranking for {_playerNumber} players.");
        }

        public void AddPlayerToRanking(Player _player)
        {
            if (_rankCount <= 0)
            {
                Debug.LogWarning($"Ranking is not supposed to go below rank 1. However, the current Rank count is {_rankCount}.");
            }
            
            _ranking[_rankCount] = _player;
            Debug.Log($"Added {_player.gameObject.name} to ranking with rank {_rankCount}.");
            _rankCount--;
        }

        public Dictionary<int, Player> Ranking => _ranking;
    }
}
