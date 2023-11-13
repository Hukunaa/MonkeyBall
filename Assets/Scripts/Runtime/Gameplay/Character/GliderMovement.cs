using Gameplay.Character;
using UnityEngine;
using CustomUtilities;
using UnityEngine.Events;

namespace Gameplay.Player
{
    [RequireComponent(typeof(TargetPracticeCharacterController))]
    public class GliderMovement : PlayerMovement
    {
        [SerializeField]
        private bool invertX;
        [SerializeField]
        private float _yawDirSmoothing;
        
        [Tooltip("In Km/h")]
        [SerializeField] 
        private float _maxSpeed;

        [Range(0, 1)]
        [SerializeField]
        private float _airResistance;
        [Range(0, 1)]
        [SerializeField]
        private float _airStaticDrag;        
        [SerializeField]
        private float _airDragOnBoost;
        [SerializeField]
        private float _climbSpeedMultiplier;
        [SerializeField]
        private float _diveSpeedMultiplier;

        [SerializeField][Range(.5f, 1)]
        private float _transformationVelocityReduction = .8f;
        
        [Range(0, 20)]
        [SerializeField]
        private float Xsensivity;
        [SerializeField]
        private float _xSnapBackSpeed;
        [SerializeField]
        private float _maxRollRotation;
        [SerializeField]
        private float _diveRate;
        [Range(0, 1)]
        [SerializeField]
        private float _collisionSpeedDecreaseFactor;

        private Vector3 _preLookDirXZ;
        private Vector3 _preLookDirY;
        private Vector3 _additionalLookDirY;
        private Vector3 _lookDir;
        private Vector3 _velocity;
        private Vector3 _additionalForces;
        private Quaternion _rotation;
        private float _angleX;
        private float _speed;
        private float _x;
        private float _inverseX;

        [SerializeField]
        private UnityEvent _onCollisionInAir;

        protected override void Awake()
        {
            base.Awake();
        }

        void Start ()
        {
            _lookDir = Vector3.forward;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rb.angularVelocity = Vector3.zero;
            var velocitySpeed = _rb.velocity.magnitude;
            var newVelocityDirection = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).normalized;
            var rot = Quaternion.LookRotation(newVelocityDirection, Vector3.up);
            _preLookDirXZ = newVelocityDirection;
            transform.rotation = rot;
            _rb.useGravity = false;
            _rb.velocity = newVelocityDirection * velocitySpeed * _transformationVelocityReduction;
            _velocity = _rb.velocity;
            _speed = _velocity.magnitude;
            _angleX = 0;
        }

        private void OnDisable()
        {
            _rb.useGravity = true;
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, _lookDir * 10, Color.red);
            Debug.DrawRay(transform.position, _preLookDirXZ * 10, Color.green);
            _preLookDirY = new Vector3(0, _rb.velocity.y, 0);
        }

        private void FixedUpdate()
        {
            _inverseX = invertX ? -1 : 1;

            _x = TargetPracticeCharacterController.HorizontalInputValue;
            _angleX += _inverseX * _x * Xsensivity * _xSnapBackSpeed;
            _angleX = Mathf.Clamp(_angleX, -_maxRollRotation, _maxRollRotation);

            if (_rb.velocity.magnitude > 0.1f)
            {
                _preLookDirXZ = MathCalculation.RotateVector(_preLookDirXZ, Vector3.up, _angleX * 0.01f);
                _lookDir = (_preLookDirXZ + (_additionalLookDirY * 2));
                _rotation = Quaternion.LookRotation(_lookDir, Vector3.up) * Quaternion.Euler(0, 0, -_angleX);
            }

            _speed = Mathf.Clamp(_speed, 0, _maxSpeed / 3.6f);

            if (_angleX != 0 && _x == 0)
                _angleX = Mathf.Lerp(_angleX, 0, Time.fixedDeltaTime * Xsensivity * _xSnapBackSpeed);

            _velocity = Vector3.Slerp(_velocity, _lookDir * _speed, 10 * Time.fixedDeltaTime);

            if (_rb)
            {
                _rb.velocity = _additionalForces + _velocity - (Vector3.up * _diveRate);
                _rb.angularVelocity = Vector3.zero;
                _rb.MoveRotation(_rotation);
            }

            _additionalForces = Vector3.Slerp(_additionalForces, Vector3.zero, _airDragOnBoost * Time.fixedDeltaTime * 10);
            _additionalLookDirY = Vector3.Slerp(_additionalLookDirY, Vector3.zero, 0.5f * Time.fixedDeltaTime * 10);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled)
            {
                Vector3 _normal = collision.GetContact(0).normal;

                float angleOfImpactDot = Vector3.Dot((_preLookDirXZ).normalized, _normal);
                angleOfImpactDot = Mathf.Clamp(angleOfImpactDot, 0, 1);
                _speed -= (1 - angleOfImpactDot) * _speed * _collisionSpeedDecreaseFactor;

                Vector3 reflectionXZ = Vector3.Reflect(_preLookDirXZ.normalized, _normal).normalized;
                //Vector3 reflectionY = Vector3.Reflect(_preLookDirY.normalized, _normal).normalized;
                _preLookDirXZ = new Vector3(reflectionXZ.x, 0, reflectionXZ.z);
                //_additionalLookDirY = new Vector3(0, reflectionY.y, 0);
                _onCollisionInAir?.Invoke();
            }
        }

        public Vector3 AdditionalForces { get => _additionalForces; set => _additionalForces = value; }
    }
}