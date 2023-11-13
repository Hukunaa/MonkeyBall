using System.Collections.Generic;
using GameplayManagers;
using UnityEngine;

namespace Gameplay.Character.AI
{
    public class AISelectTargetState : IAIState
    {
        private AIController _aiController;
        private AIStateMachine _stateMachine;
        private TargetPracticeCharacterController targetPracticeCharacterController;

        public AISelectTargetState(AIController _aiController,  AIStateMachine _stateMachine)
        {
            this._aiController = _aiController;
            this._stateMachine = _stateMachine;
            targetPracticeCharacterController = _aiController.TargetPracticeCharacterController;
        }
        
        public void Enter()
        {
            var position = _aiController.transform.position;
            Vector2 pos2D = new Vector2(position.x, position.z);
            
            var velocity = _aiController.Rigidbody.velocity;
            Vector2 horizontalVelocity = new Vector2(velocity.x,
                velocity.z).normalized;

            var bounceTargetAvailable = GetTargetsFacingMovingDirection(TargetManager.Instance.Targets[TargetManager.ETargetType.BouncePad], pos2D,
                horizontalVelocity);

            if (bounceTargetAvailable.Length > 0)
            {
                if (ShouldSelectMultiplierTarget())
                {
                    var multiplierTargetAvailable = GetTargetsFacingMovingDirection(TargetManager.Instance.Targets[TargetManager.ETargetType.Multiplier], pos2D,
                        horizontalVelocity);
                    GameObject[] targetsInRadius = GetTargetsInRadius(multiplierTargetAvailable, pos2D,
                        _aiController.Settings.TargetSearchRadius.x, _aiController.Settings.TargetSearchRadius.y);
                    
                    if (targetsInRadius.Length == 0)
                    {
                        Debug.LogError("Cannot find a target in radius.");
                        return;
                    }
                    
                    var multiplierTargetRandomIndex = Random.Range(0, targetsInRadius.Length);
                    _aiController.SetTarget(TargetManager.ETargetType.Multiplier, targetsInRadius[multiplierTargetRandomIndex]);
                    _stateMachine.TransitionTo(_stateMachine.GlidingState);
                }

                else
                {
                    GameObject[] targetsInRadius = GetTargetsInRadius(bounceTargetAvailable, pos2D,
                        _aiController.Settings.TargetSearchRadius.x, _aiController.Settings.TargetSearchRadius.y);
                    
                    if (targetsInRadius.Length == 0)
                    {
                        Debug.LogError("Can't find a Bounce pad target.");
                        return;
                    }
            
                    var bouncePadTargetRandomIndex = Random.Range(0, targetsInRadius.Length);
                    _aiController.SetTarget(TargetManager.ETargetType.BouncePad, targetsInRadius[bouncePadTargetRandomIndex]);
                    _stateMachine.TransitionTo(_stateMachine.GlidingState);
                    return;
                }
            }

            else
            {
                var multiplierTargetAvailable = GetTargetsFacingMovingDirection(TargetManager.Instance.Targets[TargetManager.ETargetType.Multiplier], pos2D,
                    horizontalVelocity);
                if (multiplierTargetAvailable.Length == 0)
                {
                    Debug.LogError("Cannot find multiplier target");
                    return;
                }
                var multiplierTargetRandomIndex = Random.Range(0, multiplierTargetAvailable.Length);
                _aiController.SetTarget(TargetManager.ETargetType.BouncePad, multiplierTargetAvailable[multiplierTargetRandomIndex]);
                _stateMachine.TransitionTo(_stateMachine.GlidingState);
            }
        }

        private bool ShouldSelectMultiplierTarget()
        {
            var random = Random.Range(0, 10);
            return random < _aiController.Settings.PickMultiplierTargetChance;
        }
        
        private GameObject[] GetTargetsFacingMovingDirection(GameObject[] availableTargets, Vector2 center, Vector2 horizontalVelocity)
        {
            List<GameObject> targetsInFront = new List<GameObject>();
            
            foreach (var target in availableTargets)
            {
                var targetPosition = target.transform.position;
                Vector2 target2DPos = new Vector2(targetPosition.x, targetPosition.z);
                
                var directionToTarget = (target2DPos - center).normalized;
                
                var dot = Vector2.Dot(horizontalVelocity, directionToTarget);

                if (dot > _aiController.Settings.SelectTargetDirectionThreshold)
                {
                    targetsInFront.Add(target);
                }
            }

            return targetsInFront.ToArray();
        }

        private GameObject[] GetTargetsInRadius(GameObject[] targets, Vector2 center, float minDistance, float maxDistance)
        {
            List<GameObject> targetsInRadius = new List<GameObject>();

            foreach (GameObject target in targets)
            {
                var targetPosition = target.transform.position;
                Vector2 target2DPos = new Vector2(targetPosition.x, targetPosition.z);
                var sqrMagnitude = (target2DPos - center).sqrMagnitude;
                if (sqrMagnitude > minDistance * minDistance && sqrMagnitude < maxDistance * maxDistance)
                {
                    targetsInRadius.Add(target);
                }
            }

            return targetsInRadius.ToArray();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}