using UnityEngine;

namespace Utilities
{
    public class FPSTargetSetter : MonoBehaviour
    {
        [SerializeField] 
        private int _targetFPS = 30;

        private void Awake()
        {
            Application.targetFrameRate = _targetFPS;
        }
    }
}
