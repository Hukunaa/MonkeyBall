using System.Linq;
using UnityEngine;

namespace Gameplay.Rewards
{
    public class DistanceCheckTarget : TargetZone
    {
        [SerializeField]
        private ScoreArea[] _targetAreas = new ScoreArea[1];
        
        private void Start()
        {
            OrderScoreArea();
        }
        
        private void OrderScoreArea()
        {
            _targetAreas = _targetAreas.OrderBy(x => x.DistanceFromCenter).ToArray();
        }

        public override int GetModifierAtPosition(Vector3 _pos)
        {
            var bodyPos = _pos;
            var targetPos = transform.position;
        
            var body2DPos = new Vector2(bodyPos.x, bodyPos.z);
            var target2DPos = new Vector2(targetPos.x, targetPos.z);

            var sqrDistance = (body2DPos - target2DPos).sqrMagnitude;

            foreach (var targetArea in _targetAreas)
            {
                if (sqrDistance <= targetArea.SqrDistanceFromCenter)
                {
                   return targetArea.Points;
                }
            }

            return 1;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (var targetArea in _targetAreas)
            {
                UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 2, 0), Vector3.up, targetArea.DistanceFromCenter);
            }
        }
        #endif
    }
}