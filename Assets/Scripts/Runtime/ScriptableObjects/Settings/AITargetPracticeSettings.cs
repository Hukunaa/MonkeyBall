using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "AITargetPracticeSettings", menuName = "ScriptableObjects/Settings/AITargetPracticeSettings", order = 0)]
    public class AITargetPracticeSettings : ScriptableObject
    {
        [SerializeField]
        private Vector2 _rollingTapRange;
        
        [SerializeField][Tooltip("The time it takes for the AI to deploy the glider after leaving the ramp.")]
        private Vector2 _gliderDeployTimeRange;
        
        [SerializeField][Range(.1f, 3)][Tooltip("The higher it is the less precise it will be in steering horizontally to reach the target while gliding")]
        private float _glidingHorizontalCapTolerance = .5f;
        
        [SerializeField]
        private Vector2 _wreckingBallTransformDistanceRange;

        [SerializeField][Range(0,10)][Tooltip("Higher value means more chance to pick a multiplier target along the way.")]
        private int _pickMultiplierTargetChance;

        [SerializeField][Range(0,1)][Tooltip("Lower value means wider search cone for targets in front of him.")]
        private float _selectTargetDirectionThreshold;

        [SerializeField]
        private Vector2 _targetSearchRadius;
        
        public Vector2 RollingTapRange => _rollingTapRange;
        
        public Vector2 GliderDeployTimeRange => _gliderDeployTimeRange;

        public float GlidingHorizontalCapTolerance => _glidingHorizontalCapTolerance;
        
        public Vector2 WreckingBallTransformDistanceRange => _wreckingBallTransformDistanceRange;
        
        public int PickMultiplierTargetChance => _pickMultiplierTargetChance;

        public float SelectTargetDirectionThreshold => _selectTargetDirectionThreshold;

        public Vector2 TargetSearchRadius => _targetSearchRadius;
    }
}