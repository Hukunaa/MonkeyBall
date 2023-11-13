using System.Linq;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace CustomCamera
{
    public class FollowPlayerInputs : MonoBehaviour
    {
        [FormerlySerializedAs("_characterController")] [FormerlySerializedAs("_playerController")] [SerializeField] 
        private TargetPracticeCharacterController _targetPracticeCharacterController;
    
        [SerializeField] 
        private CinemachineRecomposer _cmRecomposer;

        [SerializeField] 
        private float _tiltAmount = 1;
    
        [SerializeField] 
        private float _dutchAmount = 1;
    
        [SerializeField] 
        private float _tiltDumping;
    
        [SerializeField] 
        private float _dutchDumping;
    
        private VirtualCameraController _virtualCameraController;
        private float _tiltVelocity;
        private float _dutchVelocity;

        private void Awake()
        {
            _virtualCameraController = GetComponent<VirtualCameraController>();
            _virtualCameraController.onTargetChanged += UpdateCharacterController;
        }

        private void OnEnable()
        {
            var players = FindObjectsOfType<Player>();
            var player = players.FirstOrDefault(x => x.IsPlayer);
            
            if (player == null)
            {
                Debug.LogError("Cannot find the player.");
                return;
            }
            
            _targetPracticeCharacterController = player.GetComponent<TargetPracticeCharacterController>();
        }

        private void UpdateCharacterController(Transform _newTarget)
        {
            _targetPracticeCharacterController = _newTarget.GetComponent<TargetPracticeCharacterController>();
        }

        private void Update()
        {
            _cmRecomposer.m_Tilt = Mathf.SmoothDamp(_cmRecomposer.m_Tilt, _targetPracticeCharacterController.VerticalInputValue * _tiltAmount, ref _tiltVelocity, _tiltDumping);
            _cmRecomposer.m_Dutch = Mathf.SmoothDamp(_cmRecomposer.m_Dutch, _targetPracticeCharacterController.HorizontalInputValue * _dutchAmount, ref _dutchVelocity, _dutchDumping);
        }
    }
}
