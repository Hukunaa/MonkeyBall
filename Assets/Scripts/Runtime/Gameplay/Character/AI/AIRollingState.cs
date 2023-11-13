using UnityEngine;

namespace Gameplay.Character.AI
{
    public class AIRollingState : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;
        private bool _waitingForNextTap;
        private float _timeBeforeNextTap;

        public AIRollingState(AIController _aiController,  AIStateMachine _stateMachine)
        {
            this._aiController = _aiController;
            this._stateMachine = _stateMachine;
            targetPracticeCharacterController = _aiController.TargetPracticeCharacterController;
        }

        public void Enter()
        {
            
        }
        
        public void Update()
        {
            if (targetPracticeCharacterController.CharacterState == TargetPracticeCharacterController.ECharacterStates.WAITING) return;
            
            if (targetPracticeCharacterController.Player.PlayerAttemptManager.OnRamp == false)
            {
                _stateMachine.TransitionTo(_stateMachine.DeployGliderState);
                return;
            }
            
            if(_waitingForNextTap == false)
            {
                _timeBeforeNextTap = Random.Range(_aiController.Settings.RollingTapRange.x,
                    _aiController.Settings.RollingTapRange.y);
                _waitingForNextTap = true;
            }

            else
            {
                _timeBeforeNextTap -= Time.deltaTime;
                if (_timeBeforeNextTap <= 0)
                {
                    targetPracticeCharacterController.RollingMovement.Run();
                    _waitingForNextTap = false;
                }
            }
        }

        public void Exit()
        {
            
        }
    }
}