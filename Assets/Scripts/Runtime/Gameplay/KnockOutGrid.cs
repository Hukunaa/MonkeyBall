using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class KnockOutGrid : MonoBehaviour
    {
        [SerializeField] 
        private IntVariable _playersCount;
        
        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayers;
    
        [SerializeField] 
        private int _columnSize;

        [SerializeField] 
        private float _columnOffset;

        [SerializeField] 
        private float _rawOffset;

        [SerializeField]
        private float _zOffset;

        [SerializeField] 
        private AssetReference _knockoutStandAssetRef;
        
        [Header("Listening to")]
        [SerializeField] 
        private VoidEventChannel _onPlayerSpawnedEventChannel;
        
        [Header("Broadcast on")]
        [SerializeField] 
        private VoidEventChannel _onStandsAssignedEventChannel;
        
        public VoidEventChannel _onStandGridGenerated;
        
        private List<KnockOutStand> _stands = new List<KnockOutStand>();

        private AsyncOperationHandle<GameObject> _loadHandle;
        private GameObject _standPrefab;
        private bool _standsGenerated;
        private int _knockedOutCount;

        private void Awake()
        {
            _onPlayerSpawnedEventChannel.onEventRaised += AssignStandToPlayers;
            _loadHandle = _knockoutStandAssetRef.LoadAssetAsync<GameObject>();
            _loadHandle.Completed += LoadHandleOnCompleted;
        }

        private void OnDestroy()
        {
            Addressables.Release(_loadHandle);
        }

        private void AssignStandToPlayers()
        {
            _onPlayerSpawnedEventChannel.onEventRaised -= AssignStandToPlayers;
            StartCoroutine(AssignStands());
        }

        private IEnumerator AssignStands()
        {
            while (_standsGenerated == false)
            {
                Debug.Log("Waiting for stands to be generated.");
                yield return null;
            }

            var players = _remainingPlayers.RemainingPlayers;

            foreach (var player in players)
            {
                var availableStands = _stands.Where(x => x.Assigned == false).ToArray();
                var randomStandIndex = Random.Range(0, availableStands.Length);
                var stand = availableStands[randomStandIndex];
                
                if (stand == null)
                {
                    Debug.LogWarning("Could not find a stand.");
                    yield break;
                }
            
                stand.AssignPlayer(player);
            }
            
            Debug.Log("Stands assigned.");
            
            _onStandsAssignedEventChannel.RaiseEvent();
        }

        private void LoadHandleOnCompleted(AsyncOperationHandle<GameObject> obj)
        {
            _loadHandle.Completed -= LoadHandleOnCompleted;
            _standPrefab = obj.Result;
            Generate();
        }
    
        [ContextMenu("Generate Grid")]
        public void Generate()
        {
            int rawCount = 0;
            int columnCount = 0;
        
            for (int i = 0; i < _playersCount.Value; i++)
            {
                var transform1 = transform;
                var position = transform1.position;
                var spawnPos = new Vector3(position.x +columnCount * _columnOffset,position.y + rawCount * _rawOffset, position.z - rawCount * _zOffset);
                var instance = Instantiate(_standPrefab, spawnPos, quaternion.identity, transform1);
                _stands.Add(instance.GetComponent<KnockOutStand>());
                if (columnCount == _columnSize - 1)
                {
                    columnCount = 0;
                    rawCount++;
                }
                else
                {
                    columnCount++;
                }
            }
            
            Debug.Log("Stands grid generated.");

            _standsGenerated = true;
            _onStandGridGenerated.RaiseEvent();
        }
        
        [ContextMenu("Destroy Grid")]
        public void DestroyGrid()
        {
            for (int i = _stands.Count - 1; i >= 0; i--)
            {
                Destroy(_stands[i].gameObject);
                _stands.RemoveAt(i);
            }
        }

        public List<KnockOutStand> Stands => _stands;
    }
}
