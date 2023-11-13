using System;
using System.Collections.Generic;
using Gameplay.Character;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class AINameRandomizer : MonoBehaviour
    {
        [SerializeField] 
        private StringListVariable _nameList;

        [SerializeField] 
        private VoidEventChannel _randomizeOnEventChannel;

        private HashSet<string> _usedPlayerNames = new HashSet<string>();

        private void Awake()
        {
            _randomizeOnEventChannel.onEventRaised += RandomizePlayerNames;
        }
        
        private void RandomizePlayerNames()
        {
            _randomizeOnEventChannel.onEventRaised -= RandomizePlayerNames;
            
            var aiPlayers = FindAIPlayers();

            foreach (var aiPlayer in aiPlayers)
            {
                aiPlayer.SetPlayerName(FindUnusedPlayerName());
            }
        }

        private string FindUnusedPlayerName()
        {
            string playerName;
            do
            {
                var randomNameIndex = Random.Range(0, _nameList.Value.Length);
                playerName = _nameList.Value[randomNameIndex];
            } while (_usedPlayerNames.Contains(playerName));

            _usedPlayerNames.Add(playerName);
            return playerName;
        }
        

        private HashSet<Player> FindAIPlayers()
        {
            var playersGO = GameObject.FindGameObjectsWithTag("Player");

            var players = new HashSet<Player>();

            foreach (var playerGO in playersGO)
            {
                var player = playerGO.GetComponent<Player>();
                if(player.IsPlayer) continue;
                players.Add(player);
            }

            return players;
        }
    }
}
