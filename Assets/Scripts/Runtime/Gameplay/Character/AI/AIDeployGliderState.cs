using UnityEngine;

namespace Gameplay.Character.AI
{
    public class AIDeployGliderState : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;

        private float _deployTime;
        private float _currentTime;
        
        public AIDeployGliderState(AIController _aiController,  AIStateMachine _stateMachine)
        {
            this._aiController = _aiController;
            this._stateMachine = _stateMachine;
            targetPracticeCharacterController = _aiController.TargetPracticeCharacterController;
        }

        public void Enter()
        {
            _currentTime = 0;
            _deployTime = Random.Range(_aiController.Settings.GliderDeployTimeRange.x, _aiController.Settings.GliderDeployTimeRange.y);
        }
        
        public void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime < _deployTime) return;
            
            _stateMachine.TransitionTo(_stateMachine.SelectTargetState);
        }
        
        public void Exit()
        {
        }
    }
}