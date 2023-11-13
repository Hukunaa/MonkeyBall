using System;
using Gameplay.Character;
using Gameplay.Player;
using UnityEngine;

namespace CustomCamera
{
    public class TiltedFollowCamera : MonoBehaviour
    {
        [SerializeField]
        private float _initialXRotation;
        [SerializeField]
        private float _maxVerticalAngle;
        [SerializeField]
        private float _maxHorizontalAngle;
        [SerializeField]
        private float _tiltSpeed;
        
        [Header("Ground Camera Settings")]
        [SerializeField]
        private bool _useFloorNormal;
    
        [SerializeField]
        private float _groundZOffset;
    
        [SerializeField] 
        private float _groundYOffset;
    
        [Header("Air Camera Settings")]
        [SerializeField] 
        private float _glidingXRotation;
    
        [SerializeField]
        private float _airZOffset;
    
        [SerializeField] 
        private float _airYOffset;
        
        private Transform mCamera;

        private TargetPracticeCharacterController targetPracticeCharacter;
    
        // Start is called before the first frame update
        void Start()
        {
            mCamera = transform.GetChild(0);                // Get Camera from child
            targetPracticeCharacter = FindObjectOfType<TargetPracticeCharacterController>();  // Find player

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,_initialXRotation) ; // Store initial x rotation
        }
        
        
        void Update()
        {
            CameraTilt();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            FollowTarget();
        }
    
        void CameraTilt()
        {
            // Rotate camera container along the x axis when tilting the joystick up or down to give a forward and back tilt effect.
            // The further up the joystick is the higher the angle for target rotation will be and vice versa.
            float scaledVerticalTilt = _initialXRotation - (targetPracticeCharacter.VerticalInputValue * _maxVerticalAngle);

            Quaternion targetXRotation;
        
            if (targetPracticeCharacter.RollingMovement.OnGround())
            {
                // Using floor normal adjust the rotation of the camera's x axis at rest.
                float angleBetweenFloorNormal = _useFloorNormal ? Vector3.SignedAngle(Vector3.up, targetPracticeCharacter.RollingMovement.FloorNormal, transform.right) : 0.0f;

                targetXRotation = Quaternion.Euler(scaledVerticalTilt + angleBetweenFloorNormal, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetXRotation, _tiltSpeed * Time.deltaTime);
            }

            else
            {
                targetXRotation = Quaternion.Euler(_glidingXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetXRotation, _tiltSpeed * Time.deltaTime);
        
            // Rotate camera along the z axis when tilting the joystick left or right to give a left and right tilt effect.
            // The further right the joystick is the higher the angle for target rotation will be and vice versa.
            float scaledHorizontalTilt = Input.GetAxis("Horizontal") * _maxHorizontalAngle;

            Quaternion targetZRotation = Quaternion.Euler(mCamera.rotation.eulerAngles.x, mCamera.rotation.eulerAngles.y, scaledHorizontalTilt);

            mCamera.rotation = Quaternion.RotateTowards(mCamera.rotation, targetZRotation, _tiltSpeed * Time.deltaTime);
        }

        void FollowTarget()
        {
            // Get forward vector minus the y component
            Vector3 vectorA = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

            // Get target's velocity vector minus the y component
            Vector3 vectorB = new Vector3(targetPracticeCharacter.Velocity.x, 0.0f, targetPracticeCharacter.Velocity.z);

            // Find the angle between vectorA and vectorB
            float rotateAngle = Vector3.SignedAngle(vectorA.normalized, vectorB.normalized, Vector3.up);

            // Get the target's speed (magnitude) without the y component
            // Only set speed factor when vector A and B are almost facing the same direction
            float speedFactor = Vector3.Dot(vectorA, vectorB) > 0.0f ? vectorB.magnitude : 1.0f;

            // Rotate towards the angle between vectorA and vectorB
            // Use speedFactor so camera doesn't rotate at a constant speed
            // Limit speedFactor to be between 1 and 2
            transform.Rotate(Vector3.up, rotateAngle * Mathf.Clamp(speedFactor, 1.0f, 2.0f) * Time.deltaTime);
            
            float zOffset = targetPracticeCharacter.RollingMovement.OnGround() ? _groundZOffset : _airZOffset;
            float yOffset = targetPracticeCharacter.RollingMovement.OnGround() ? _groundYOffset : _airYOffset;
        
            Vector3 targetPosition = targetPracticeCharacter.transform.position + Vector3.up * yOffset;
            // Position the camera behind target at a distance of offset
            transform.position = targetPosition - (transform.forward * zOffset);
        
            transform.LookAt(targetPosition, Vector3.up);
        }
    }
}
