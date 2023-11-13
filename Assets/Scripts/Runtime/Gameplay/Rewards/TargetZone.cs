using UnityEngine;

namespace Gameplay.Rewards
{
    public abstract class TargetZone : MonoBehaviour
    {
        public abstract int GetModifierAtPosition(Vector3 _pos);
    }
}