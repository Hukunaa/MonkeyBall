using System.Linq;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CustomCamera
{
    public class SpectatingCameraManager : MonoBehaviour
    {
        [SerializeField] 
        private TransformEventChannel _onChangeSpectatingTargetEventChannel;

        [SerializeField] 
        private VoidEventChannel _onDeactivateSpectatingCamera;
    
        [SerializeField]
        private RemainingPlayersContainer _remainingPlayersContainer;

        [SerializeField] 
        private BoolVariable _isAttemptInProgress;
        
        [SerializeField] 
        private bool _spectateOnlyPlayerStillPlaying;

        [SerializeField] 
        private UnityEvent<Transform> _onChangeTarget;
        
        private VirtualCameraController _vcController;
        
        private Transform _currentTarget;
    
        private void Awake()
        {
            _vcController = GetComponentInChildren<VirtualCameraController>();
            _onChangeSpectatingTargetEventChannel.onEventRaised += OnPlayerKnockedOut;
            _onDeactivateSpectatingCamera.onEventRaised += DeactivateCamera;
        }

        private void OnDestroy()
        {
            _onChangeSpectatingTargetEventChannel.onEventRaised -= OnPlayerKnockedOut;
        }

        public void ActivateCamera()
        {
            _vcController.SetCameraActive();
        }

        public void DeactivateCamera()
        {
            _vcController.SetCameraInactive();
        }

        private void OnPlayerKnockedOut(Transform _playerTransform)
        {
            if (_playerTransform == _currentTarget)
            {
                ChangeSpectatingTarget();
            }
        }

        public void ChangeSpectatingTarget()
        {
            _currentTarget = FindNewTarget();
            if (_currentTarget == null) return;
            
            _onChangeTarget?.Invoke(_currentTarget);
        }

        private Transform FindNewTarget()
        {
            var targets = _isAttemptInProgress.Value && _spectateOnlyPlayerStillPlaying
                ? _remainingPlayersContainer.RemainingPlayers.Where(x => x.PlayerAttemptManager.PlayerTurnInProgress)
                    .Select(x => x.transform).ToArray()
                : _remainingPlayersContainer.RemainingPlayers.Select(x => x.transform).ToArray();

            return targets.Length == 0 ? _currentTarget : GetRandomTarget(targets);
        }
        
        private Transform GetRandomTarget(Transform[] _availableTargets)
        {
            if (_availableTargets.Length == 1)
            {
                return _availableTargets[0];
            }

            Transform target;
            
            do
            {
                var targetIndex = Random.Range(0, _availableTargets.Length);
                target = _availableTargets[targetIndex];
            } while (_currentTarget == target);
            
            return target;
        }
    }
}
