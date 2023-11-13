using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCamera
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [SerializeField] 
        private VirtualCameraController _orbitCamera;
        
        [SerializeField] 
        private VirtualCameraController _rollingCamera;
        
        [SerializeField] 
        private VirtualCameraController _glidingCamera;
        
        [SerializeField] 
        private VirtualCameraController _wreckingCamera;

        [SerializeField] 
        private UnityEvent _onActivatePlayerCamera;
        
        [SerializeField]
        private UnityEvent<Transform> _onNewTargetAssigned;
        
        private Transform _target;
        private VirtualCameraController _currentCamera;

        private void Start()
        {
            _currentCamera = _rollingCamera;
        }

        [ContextMenu("Change Target")]
        public void ChangeTarget(Transform _newTarget)
        {
            _target = _newTarget;
            
            _onNewTargetAssigned?.Invoke(_newTarget);
        }

        public void ActivatePlayerCamera()
        {
            _onActivatePlayerCamera?.Invoke();
        }
        
        public void DeactivatePlayerCamera()
        {
            Debug.Log("Deactivate Player Camera");
            ResetCurrentCamera();
        }
        
        public void SetOrbitCameraActive()
        {
            ResetCurrentCamera();
            SetCameraActive(_orbitCamera);
        }

        public void SetRollingCameraActive()
        {
            ResetCurrentCamera();
            SetCameraActive(_rollingCamera);
        }

        public void SetGlidingCameraActive()
        {
            ResetCurrentCamera();
            SetCameraActive(_glidingCamera);
        }
        
        public void SetWreckingCameraActive()
        {
            ResetCurrentCamera();
            SetCameraActive(_wreckingCamera);
        }
        
        public void ResetCurrentCamera()
        {
            if (_currentCamera != null)
            {
                SetCameraInactive(_currentCamera);
                _currentCamera = null;
            }
        }

        private void SetCameraInactive(VirtualCameraController _vcc)
        {
            _vcc.SetCameraInactive();
        }

        private void SetCameraActive(VirtualCameraController _vcc)
        {
            _currentCamera = _vcc;
            _vcc.SetCameraActive();
        }

        public void TeleportPlayer(Vector3 _positionDelta)
        {
            if (_currentCamera == null) return;
            _currentCamera.WrapCamera(_positionDelta);
        }

        public Transform Target => _target;
    }
}