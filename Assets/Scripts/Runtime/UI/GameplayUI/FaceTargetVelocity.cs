using UnityEngine;

namespace UI.GameplayUI
{
    public class FaceTargetVelocity : MonoBehaviour
    {
        [SerializeField] 
        private bool _ignoreY = true;
        
        private Transform _target;
        private Rigidbody _targetRb;
        
        public void SetTarget(Transform _newTarget)
        {
            _target = _newTarget;
            _targetRb = _target.GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (_targetRb == null) return;
            
            var velocity = _targetRb.velocity;
            var forwardVector = velocity == Vector3.zero ? _target.forward : new Vector3(velocity.x, _ignoreY? 0 : velocity.y, velocity.z).normalized;
            transform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        }
    }
}