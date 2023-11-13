using System.Collections.Generic;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.DataContainer
{
    [CreateAssetMenu(fileName = "RemainingPlayerContainer", menuName = "ScriptableObjects/DataContainer/RemainingPlayersContainer", order = 0)]
    public class RemainingPlayersContainer : ScriptableObject
    {
        [SerializeField]
        private List<Player> _remainingPlayers;

        public UnityAction OnRemainingPlayersChanged;

        public void Initialize(List<Player> _players)
        {
            _remainingPlayers = _players;
            OnRemainingPlayersChanged?.Invoke();
        }

        public void AddPlayer(Player _player)
        {
            if (_remainingPlayers.Contains(_player)) return;
            
            _remainingPlayers.Add(_player);
            OnRemainingPlayersChanged?.Invoke();
        }

        public void RemovePlayer(Player _player)
        {
            if(!_remainingPlayers.Contains(_player)) return;
            
            _remainingPlayers.Remove(_player);
            OnRemainingPlayersChanged?.Invoke();
        }

        public void Clear()
        {
            _remainingPlayers.Clear();
        }

        public List<Player> RemainingPlayers => _remainingPlayers;

        public int RemainingPlayersCount
        {
            get
            {
                if (_remainingPlayers == null) return 0;
                return _remainingPlayers.Count;
            }
        }
    }
}