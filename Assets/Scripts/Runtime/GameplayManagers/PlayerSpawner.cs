using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Character;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameplayManagers
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;
        
        [SerializeField] 
        private AssetReference _playerAssetRef;
    
        [SerializeField] 
        private AssetReference _aiPlayerAssetRef;

        [SerializeField] 
        private IntVariable _playersCount;

        [SerializeField] 
        private VoidEventChannel _onPlayersSpawnedEventChannel;

        [SerializeField] 
        private string _playerName = "Player";

        [SerializeField] 
        private string _aiPlayerName = "AIPlayer";

        [SerializeField][Tooltip("Set to true if you prefer to spawn the players manually. It will find the players and invoke the event.")]
        private bool _fakeSpawns;

        [SerializeField] 
        private Vector3 _spawnPosition;
        
        private AsyncOperationHandle<GameObject> _playerAssetRefLoadHandle;
        private AsyncOperationHandle<GameObject> _aiPlayerAssetRefLoadHandle;

        private GameObject _playerPrefab;
        private GameObject _aiPlayerPrefab;
        
        private List<Player> _players = new List<Player>();

        private void Start()
        {
            if (_fakeSpawns)
            {
                _players = FindObjectsOfType<Player>().ToList();
                CompleteSpawnProcess();
                return;
            }
            
            StartCoroutine(Initialize());
        }

        private void OnDestroy()
        {
            if (_fakeSpawns) return;
            
            Addressables.Release(_playerAssetRefLoadHandle);
            Addressables.Release(_aiPlayerAssetRefLoadHandle);
        }

        private IEnumerator Initialize()
        {
            _playerAssetRefLoadHandle = _playerAssetRef.LoadAssetAsync<GameObject>();
            _aiPlayerAssetRefLoadHandle = _aiPlayerAssetRef.LoadAssetAsync<GameObject>();
            
            yield return _playerAssetRefLoadHandle;
            _playerPrefab = _playerAssetRefLoadHandle.Result;

            yield return _aiPlayerAssetRefLoadHandle;
            _aiPlayerPrefab = _aiPlayerAssetRefLoadHandle.Result;
        
            SpawnPlayers();
            
            CompleteSpawnProcess();
        }

        private void CompleteSpawnProcess()
        {
            _remainingPlayersContainer.Initialize(_players);
            _onPlayersSpawnedEventChannel.RaiseEvent();
        }
        
        private void SpawnPlayers()
        {
            for (int i = 0; i < _playersCount.Value; i++)
            {
                var instance = Instantiate(i == 0? _playerPrefab : _aiPlayerPrefab, new Vector3(_spawnPosition.x + i, _spawnPosition.y, _spawnPosition.z), Quaternion.identity);

                instance.name = i == 0 ? _playerName : $"{_aiPlayerName}{i}";
            
                _players.Add(instance.GetComponent<Player>());
            }
        }
    }
}
