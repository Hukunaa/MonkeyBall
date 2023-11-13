using System;

namespace Gameplay.Character.AI
{
    [Serializable]
    public class AIStateMachine
    {
        public IAIState CurrentState { get; private set; }

        private AIController _aiController;

        private IAIState _idleState;
        private IAIState _rollingState;
        private IAIState _deployGliderState;
        private IAIState _glidingState;
        private IAIState _wreckingBallState;
        private IAIState _selectTargetState;

        public AIStateMachine(AIController aiController)
        {
            this._aiController = aiController;

            _idleState = new AIIdleState(aiController, this);
            _rollingState = new AIRollingState(aiController, this);
            _deployGliderState = new AIDeployGliderState(aiController, this);
            _glidingState = new AIGlidingState(aiController, this);
            _wreckingBallState = new AIWreckingBall(_aiController, this);
            _selectTargetState = new AISelectTargetState(_aiController, this);
        }
           
        public void Initialize(IAIState startingState)
        {
            CurrentState?.Exit();
            CurrentState = startingState;
            startingState.Enter();
        }
        
        public void TransitionTo(IAIState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }
        
        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        public IAIState IdleState => _idleState;

        public IAIState RollingState => _rollingState;

        public IAIState DeployGliderState => _deployGliderState;

        public IAIState GlidingState => _glidingState;

        public IAIState WreckingBallState => _wreckingBallState;

        public IAIState SelectTargetState => _selectTargetState;
    }
}