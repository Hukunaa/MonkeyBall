using System;
using System.Linq;
using Gameplay.Character;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace GameplayManagers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] 
        private SpawnPoint[] _spawnPoints;

        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;
        
        [Header("Broadcasting on")]
        [SerializeField] 
        private VoidEventChannel _onSpawnPointsInitialized;
    
        private AsyncOperationHandle<GameplaySettings> _gameplaySettingsLoadHandle;
        private GameplaySettings _gameplaySettings;
        
        public void AssignSpawnPointsToPlayers()
        {
            var players = _remainingPlayersContainer.RemainingPlayers.Select(x => x.GetComponent<PlayerRespawn>()).ToArray();
            GetSpawnPoints();

            foreach (var player in players)
            {
                AssignSpawnPointToPlayer(player);
            }
        
            _onSpawnPointsInitialized.RaiseEvent();
        }

        private void GetSpawnPoints()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.GetComponent<SpawnPoint>()).ToArray();
            
        }

        private void AssignSpawnPointToPlayer(PlayerRespawn _player)
        {
            var availableSpawnPoints = _spawnPoints.Where(x => x.Assigned == false).ToArray();
       
        
            if (availableSpawnPoints.Length == 0)
            {
                Debug.LogWarning("Can't assign spawn point because there is no spawn point available.");
                return;
            }
        
            int randomIndex = availableSpawnPoints.Length > 1 ? Random.Range(0, availableSpawnPoints.Length) : 0;
            availableSpawnPoints[randomIndex].AssignPlayer(_player);
            _player.AssignSpawnPoint(availableSpawnPoints[randomIndex]);
        }
    }
}
