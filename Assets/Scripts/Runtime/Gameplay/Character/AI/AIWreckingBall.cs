using System;
using GameplayManagers;
using UnityEngine;

namespace Gameplay.Character.AI
{
    public class AIWreckingBall : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;

        private float wreckingBallDistance;
        
        public AIWreckingBall(AIController _aiController,  AIStateMachine _stateMachine)
        {
            this._aiController = _aiController;
            this._stateMachine = _stateMachine;
            targetPracticeCharacterController = _aiController.TargetPracticeCharacterController;
        }

        public void Enter()
        {
            targetPracticeCharacterController.ActiveWreckingBall();
            switch (_aiController.CurrentTargetType)
            {
                case TargetManager.ETargetType.BouncePad:
                    _aiController.PlayerBoost._onPlayerGotBounced += OnPlayerBounced;
                    break;
                case TargetManager.ETargetType.BounceRing:
                    break;
                case TargetManager.ETargetType.Multiplier:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPlayerBounced()
        {
            _aiController.PlayerBoost._onPlayerGotBounced -= OnPlayerBounced;
            _stateMachine.TransitionTo(_stateMachine.DeployGliderState);
        }

        public void Update()
        {
            
        }
        
        public void Exit()
        {
            _aiController.PlayerBoost._onPlayerGotBounced -= OnPlayerBounced;
        }
        
    }
}