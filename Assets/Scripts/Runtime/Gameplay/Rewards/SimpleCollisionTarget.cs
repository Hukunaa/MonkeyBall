using UnityEngine;

namespace Gameplay.Rewards
{
    public class SimpleCollisionTarget : TargetZone
    {
        [SerializeField] 
        private int _points;
        
        public override int GetModifierAtPosition(Vector3 _pos)
        {
            return _points;
        }
    }
}
