using System;
using Gameplay.Player;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Character
{
    public class TargetPracticeCharacterController : MonoBehaviour
    {
        public enum ECharacterStates
        {
            WAITING,
            ROLLING,
            GLIDING,
            WRECKING,
        }
        
        [SerializeField]
        private PlayerAnimator _playerAnimator;
    
        [SerializeField] 
        private RollingMovement _rollingMovement;

        [SerializeField] 
        private GliderMovement _gliderMovement;
        
        [SerializeField] 
        private WreckingBallMovement _wreckingBallMovement;

        private PlayerMovement _currentMovement;
        
        [SerializeField] 
        private Rigidbody _rigidbody;
        
        [SerializeField] 
        private float _glidingToWreckingSpeedThreshold = 1;

        [SerializeField] private float _defaultMass;
        [SerializeField] private float _defaultDrag;
        [SerializeField] private float _defaultAngularDrag;
        
        [FormerlySerializedAs("_state")][SerializeField]
        private ECharacterStates _characterState;

        [SerializeField] 
        private Transform _characterTransform;

        private Player _player;
        
        private float horizontalInputValue;
        private float verticalInputValue;
        
        private bool _onTarget;

        public Action<ECharacterStates> onStateChange;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _characterState = ECharacterStates.WAITING;
            _playerAnimator.ChangeAnimationState(ANIMATION_STATE.IDLE);
        }

        public void Initialize()
        {
            ResetController();
            ActivateRolling();
        }
        
        public void ResetController()
        {
            DisableAllMovements();
            ResetCachedInputValues();
            ResetRigidbody();
            _characterTransform.localRotation = Quaternion.identity;
        }

        private void ResetRigidbody()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.mass = _defaultMass;
            _rigidbody.drag = _defaultDrag;
            _rigidbody.angularDrag = _defaultAngularDrag;
            _rigidbody.freezeRotation = false;
        }

        private void ResetCachedInputValues()
        {
            horizontalInputValue = 0;
            verticalInputValue = 0;
        }
        
        public void DisableAllMovements()
        {
            ChangeState(ECharacterStates.WAITING);
            
            _rollingMovement.enabled = false;
            _gliderMovement.enabled = false;
            _wreckingBallMovement.enabled = false;
            _currentMovement = null;
        }

        private void Update()
        {
            if (_characterState == ECharacterStates.GLIDING && _rigidbody.velocity.sqrMagnitude <=
                _glidingToWreckingSpeedThreshold * _glidingToWreckingSpeedThreshold)
            {
                Debug.Log($"{gameObject.name} pass under speed threshold. Transform into Wrecking ball.");
                ActiveWreckingBall();
            }
        }
        
        public void ChangeState(ECharacterStates _newState)
        {
            _characterState = _newState;
            onStateChange?.Invoke(_newState);
        }
        
        public void ActivateRolling()
        {
            ChangeState(ECharacterStates.ROLLING);
            ChangeActiveMovement(_characterState);
            _playerAnimator.ChangeAnimationState(ANIMATION_STATE.ROLLING);
        }

        private void ChangeActiveMovement(ECharacterStates _state)
        {
            switch (_state)
            {
                case ECharacterStates.WAITING:
                    if (_currentMovement != null)
                    {
                        _currentMovement.enabled = false;
                        _currentMovement = null;
                    }
                    break;
                case ECharacterStates.ROLLING:
                    UpdateCurrentMovement(_rollingMovement);
                    break;
                case ECharacterStates.GLIDING:
                    UpdateCurrentMovement(_gliderMovement);
                    break;
                case ECharacterStates.WRECKING:
                    UpdateCurrentMovement(_wreckingBallMovement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
        }

        private void UpdateCurrentMovement(PlayerMovement _newMovement)
        {
            if (_currentMovement != null)
            {
                _currentMovement.enabled = false;
            }
            
            _currentMovement = _newMovement;
            _currentMovement.enabled = true;
        }
    
        public void ActivateGlider()
        {
            if (GliderTransformationEnabled == false || _characterState == ECharacterStates.GLIDING) return;
            
            ChangeState(ECharacterStates.GLIDING);
            ChangeActiveMovement(_characterState);
            _playerAnimator.ChangeAnimationState(ANIMATION_STATE.FLYING);
        }

        public void ActiveWreckingBall()
        {
            if (_characterState == ECharacterStates.WRECKING) return;
            ChangeState(ECharacterStates.WRECKING);
            ChangeActiveMovement(_characterState);
            _playerAnimator.ChangeAnimationState(ANIMATION_STATE.ROLLING);
        }

        public void UpdateJoystickInput(Vector2 _newValue)
        {
            horizontalInputValue = _newValue.x;
            verticalInputValue = _newValue.y;
        }
        
        public float HorizontalInputValue
        {
            get => horizontalInputValue;
            set => horizontalInputValue = value;
        }

        public float VerticalInputValue
        {
            get => verticalInputValue;
            set => verticalInputValue = value;
        }

        public Player Player => _player;

        public Vector3 Velocity => _rigidbody.velocity;

        public RollingMovement RollingMovement => _rollingMovement;

        public GliderMovement GliderMovement => _gliderMovement;

        public ECharacterStates CharacterState { get => _characterState; }

        public bool GliderTransformationEnabled { get; set; }
    }
}
