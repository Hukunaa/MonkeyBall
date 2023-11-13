using System;
using Gameplay.Player;
using GeneralScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Character
{
    public class RollingMovement : PlayerMovement
    {
        [SerializeField]
        private UnityEvent _onBoost;
        
        [SerializeField]
        private LayerMask whatIsGround;
        
        [SerializeField]
        private float groundCheckRadius;

        [SerializeField] 
        private EForceType _forceType = EForceType.Torque;

        [SerializeField]
        private FloatVariable torqueStrength;

        [SerializeField]
        private FloatVariable boostTorqueStrength;

        [SerializeField]
        private FloatVariable _attemptStartBoostTimeFrame;

        [SerializeField] 
        private UnityEvent _onStartBoostTriggered;

        private enum EForceType
        {
            Force,
            Torque
        }
        
        
        private Vector3 floorNormal;
        private bool _runThisFrame;
        private bool _startBoostActive;
        private float _remainingBoostTime;

        protected override void Awake()
        {
            base.Awake();
            TargetPracticeCharacterController = GetComponent<TargetPracticeCharacterController>();
            _rb = GetComponent<Rigidbody>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rb.mass = _mass;
        }

        public void StartBoostTimeFrame()
        {
            _remainingBoostTime = _attemptStartBoostTimeFrame.Value;
            _startBoostActive = true;
        }

        private void Update()
        {
            if (!_startBoostActive) return;
            
            _remainingBoostTime -= Time.deltaTime;
            if (_remainingBoostTime <= 0)
            {
                _startBoostActive = false;
            }
        }

        private void FixedUpdate()
        {
            if (_runThisFrame)
            {
                switch (_forceType)
                {
                    case EForceType.Force:
                        _rb.AddForce(CalculateFloorTangent() * torqueStrength.Value, ForceMode.Force);
                        break;
                    case EForceType.Torque:
                        _rb.AddTorque(transform.right * torqueStrength.Value, ForceMode.Force);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
                if (_startBoostActive)
                {
                    _rb.AddForce(CalculateFloorTangent() * boostTorqueStrength.Value, ForceMode.Impulse);
                    Debug.Log("Rolling boost triggered");
                    _onStartBoostTriggered?.Invoke();
                }
            }

            _runThisFrame = false; 
        }

        public void Run()
        {
            _runThisFrame = true;
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Boost"))
            {
                _onBoost.Invoke();
            }
        }

        private Vector3 CalculateFloorTangent()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
            {
                floorNormal = hit.normal;
                return Vector3.Cross(transform.right, floorNormal);
            }
            
            return Vector3.zero;
        }
        
        public bool OnGround()
        {
            return Physics.CheckSphere(transform.position, groundCheckRadius, whatIsGround);
        }

        public Vector3 FloorNormal => floorNormal;
    }
}