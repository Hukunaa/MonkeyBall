using CustomUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Character.AI
{
    public class AIGlidingState : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;

        private float wreckingBallDistance;

        public AIGlidingState(AIController _aiController,  AIStateMachine _stateMachine)
        {
            this._aiController = _aiController;
            this._stateMachine = _stateMachine;
            targetPracticeCharacterController = _aiController.TargetPracticeCharacterController;
        }

        public void Enter()
        {
            targetPracticeCharacterController.ActivateGlider();
            wreckingBallDistance = Random.Range(_aiController.Settings.WreckingBallTransformDistanceRange.x,
                _aiController.Settings.WreckingBallTransformDistanceRange.y);
        }
        
        public void Update()
        {
            var targetPos = _aiController.CurrentTarget.transform.position;
            var characterPos = targetPracticeCharacterController.transform.position;
            var target2DPos = new Vector2(targetPos.x, targetPos.z);
            var character2DPos = new Vector2(characterPos.x,
                characterPos.z);
            var targetDirection = target2DPos - character2DPos;
            var targetNormalizedDirection = targetDirection.normalized;
            var targetDistance = targetDirection.sqrMagnitude;

            var characterForward = targetPracticeCharacterController.transform.forward;
            var characterForward2D =
                new Vector2(characterForward.x, characterForward.z).normalized;
            
            var direction =  Vector2.SignedAngle(characterForward2D, targetNormalizedDirection);
            
            SetHorizontalInput(direction);

            if (targetDistance <= wreckingBallDistance * wreckingBallDistance)
            {
                _stateMachine.TransitionTo(_stateMachine.WreckingBallState);
            }
        }

        private void SetHorizontalInput(float _angle)
        {
            if (MathCalculation.ApproximatelyEqualFloat(_angle, 0, _aiController.Settings.GlidingHorizontalCapTolerance))
            {
                targetPracticeCharacterController.HorizontalInputValue = 0;
            }
            else if (_angle < 0)
            {
                targetPracticeCharacterController.HorizontalInputValue = 1;
            }
            else
            {
                targetPracticeCharacterController.HorizontalInputValue = -1;
            }
        }
        
        public void Exit()
        {
            
        }
    }
}