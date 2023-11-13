using System.Linq;
using GeneralScriptableObjects;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameplayManagers
{
    public class AttemptCompleteManager : MonoBehaviour
    {
        [SerializeField] 
        private float _velocityThreshold;

        [SerializeField] 
        private float _attemptTimerDuration;
        
        [SerializeField] 
        private FloatVariable _attemptTimerVariable;
        
        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;
        
        [FormerlySerializedAs("_onRoundComplete")] public UnityEvent _onAttemptComplete;
        
        private bool _checkEndCondition;
        
        public void ResetTimer()
        {
            _attemptTimerVariable.SetValue(_attemptTimerDuration);
        }
        
        public void StartCheck()
        {
            _checkEndCondition = true;
        }
        
        private void Update()
        {
            if (_checkEndCondition == false) return;

            var timer = Mathf.Clamp(_attemptTimerVariable.Value - Time.deltaTime, 0, _attemptTimerDuration);
            _attemptTimerVariable.SetValue(timer);
            
            if (timer == 0 || (DidPlayersLeftPlatform() && DidPlayersStopped()))
            {
                Debug.Log("Attempt completed!");
                AttemptComplete();
            }
        }

        private bool DidPlayersLeftPlatform()
        {
            return _remainingPlayersContainer.RemainingPlayers.All(_player => _player.PlayerAttemptManager.OnRamp == false);
        }

        private bool DidPlayersStopped()
        {
            return _remainingPlayersContainer.RemainingPlayers.All(_player => _player.Rb.velocity.sqrMagnitude < _velocityThreshold * _velocityThreshold);
        }

        private void AttemptComplete()
        {
            _checkEndCondition = false;
            Debug.Log("End of Round.");
            _attemptTimerVariable.SetValue(0);
            _onAttemptComplete?.Invoke();
        }
    }
}
