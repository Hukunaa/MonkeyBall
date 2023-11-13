using System.Linq;
using Gameplay.Character;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class UITargetSetter : MonoBehaviour
    {
        [SerializeField] 
        private bool _initializeOnStart;
        
        [SerializeField] 
        private VoidEventChannel _onPlayerSpawnedEventChannel;

        public UnityEvent<Transform> _onTargetFound;

        private void Awake()
        {
            _onPlayerSpawnedEventChannel.onEventRaised += InitializeUITarget;
        }

        private void OnDestroy()
        {
            _onPlayerSpawnedEventChannel.onEventRaised -= InitializeUITarget;
        }

        private void Start()
        {
            if (_initializeOnStart)
            {
                InitializeUITarget();
            }
        }

        private void InitializeUITarget()
        {
            var players = FindObjectsOfType<Player>();
            var player = players.FirstOrDefault(x => x.IsPlayer);
            
            if (player == null)
            {
                Debug.LogWarning("Can't find Player.");
                return;
            }
            
            SetUITarget(player.transform);
        }

        private void SetUITarget(Transform _targetTransform)
        {
            _onTargetFound?.Invoke(_targetTransform);
        }
    }
}
