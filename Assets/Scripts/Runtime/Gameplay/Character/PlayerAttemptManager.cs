using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerAttemptManager : MonoBehaviour
    {
        [SerializeField] 
        private float _attemptEndVelocityThreshold;
        
        [SerializeField] 
        private float _raycastDistance = .55f;
        
        [SerializeField] 
        private LayerMask _targetLayerMask;

        public UnityEvent OnPlayerTurnEnd;
        public UnityEvent OnPlayerFallIntoWater;
        public UnityEvent OnPlayerEndTurnOnTarget;
        public UnityEvent onLeaveRamp;
    
        private Rigidbody _rb;

        private bool _playerTurnInProgress;
        private bool _onRamp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (_playerTurnInProgress == false || _onRamp || _rb.velocity.sqrMagnitude > _attemptEndVelocityThreshold * _attemptEndVelocityThreshold) return;
            
            PlayerTurnComplete();
        }
        
        public void PlayerTurnStart()
        {
            _onRamp = true;
            _playerTurnInProgress = true;
        }
    
        public void PlayerTurnComplete()
        {
            if (_playerTurnInProgress == false) return;
            
            Debug.Log($"{gameObject.name} turn complete.");
            _playerTurnInProgress = false;
            
            if (ReachedTarget)
            {
                OnPlayerEndTurnOnTarget?.Invoke();
            }
            
            OnPlayerTurnEnd?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enabled == false || other.CompareTag("Water") == false) return;
            FallInWater();
        }

        private void FallInWater()
        {
            OnPlayerFallIntoWater?.Invoke();
            PlayerTurnComplete();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_playerTurnInProgress && other.CompareTag("PlatformEnd") == true)
            {
                LeaveRamp();
            }
        }

        private void LeaveRamp()
        {
            _onRamp = false;
            onLeaveRamp?.Invoke();
        }

        public bool ReachedTarget => Physics.Raycast(transform.position, Vector3.down, _raycastDistance, _targetLayerMask);
        public bool PlayerTurnInProgress => _playerTurnInProgress;
        public bool OnRamp => _onRamp;
    }
}
