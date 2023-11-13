using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class FloatingMovement : PlayerMovement
    {
        public float _waterLevel = 0.0f;
        public float _floatThreshold = 2.0f;
        public float _waterDensity = 0.125f;
        public float _downForce = 4.0f;

        private float forceFactor;
        private Vector3 floatForce;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _rb.angularVelocity = Vector3.zero;
        }

        void FixedUpdate () {
            forceFactor = 1.0f - ((transform.position.y - _waterLevel) / _floatThreshold);

            if (forceFactor > 0.0f) {
                floatForce = -Physics.gravity *  _rb.mass * (forceFactor - _rb.velocity.y * _waterDensity);
                floatForce += new Vector3 (0.0f, -_downForce * _rb.mass, 0.0f);
                _rb.AddForceAtPosition (floatForce, transform.position);
            }
        }
    }
}
