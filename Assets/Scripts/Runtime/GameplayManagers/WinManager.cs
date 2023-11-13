using GeneralScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameplayManagers
{
    public class WinManager : MonoBehaviour
    {
        [FormerlySerializedAs("_playerManager")] [SerializeField] 
        private KnockOutManager _knockOutManager;
        
        [SerializeField] 
        private BoolVariable _isAttemptInProgress;
        
        [SerializeField] 
        private UnityEvent _onWinnerFound;
        
        [SerializeField] 
        private UnityEvent _onNoWinnerFound;
        
        public void CheckWinCondition()
        {
            if (_knockOutManager.RemainingPlayerCount == 1)
            {
                _onWinnerFound?.Invoke();
            }

            else
            {
                _onNoWinnerFound?.Invoke();
            }
        }
        
        private void Update()
        {
            if (_isAttemptInProgress.Value == true)
            {
                if (_knockOutManager.RemainingPlayerCount == 1)
                {
                    _onWinnerFound?.Invoke();
                }
            }
        }
    }
}