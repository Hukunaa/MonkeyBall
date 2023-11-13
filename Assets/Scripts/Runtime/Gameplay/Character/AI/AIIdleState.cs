namespace Gameplay.Character.AI
{
    public class AIIdleState : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;
        
        public AIIdleState(AIController _aiController, AIStateMachine _stateMachine)
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
            if (targetPracticeCharacterController.CharacterState != TargetPracticeCharacterController.ECharacterStates.ROLLING) return;
            _stateMachine.TransitionTo(_stateMachine.RollingState);
        }
        
        public void Exit()
        {
            
        }
    }
}