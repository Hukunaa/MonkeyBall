using System.Linq;
using GeneralScriptableObjects;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class GameOverUIManager : MonoBehaviour
    {
        [SerializeField] 
        private bool _skipOnLastRound;

        [SerializeField][Range(1, 5)]
        private int _maxRoundCount;
    
        [SerializeField] 
        private IntVariable _roundCount;

        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;
    
        [SerializeField] 
        private UnityEvent _onDisplayGameOverScreen;
    
        private bool _gameOverVisible;
    
        public void CheckIfPlayerWasEliminated()
        {
            if (_gameOverVisible || _remainingPlayersContainer.RemainingPlayers.Any(x => x.IsPlayer)) return;
        
            DisplayGameOverScreen();
            _gameOverVisible = true;
        }
    
        public void DisplayGameOverScreen()
        {
            if (_skipOnLastRound && _roundCount.Value == _maxRoundCount) return;
        
            _onDisplayGameOverScreen?.Invoke();
        }
    }
}
