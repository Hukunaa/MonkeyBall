using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayManagers
{
    public class TutorialEndConditionChecker : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent _onTutorialEnd;
        
        [SerializeField] 
        private UnityEvent _onTutorialContinue;
        
        public void CheckTutorialCompleteCondition()
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            
            if (player.PlayerAttemptManager.ReachedTarget)
            {
                _onTutorialEnd?.Invoke();
                return;
            }
            
            _onTutorialContinue?.Invoke();
        }
    }
}