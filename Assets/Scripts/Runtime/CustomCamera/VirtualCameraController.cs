using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCamera
{
    public class VirtualCameraController : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineVirtualCamera _virtualCamera;

        [SerializeField]
        private bool _isBlendlistCamera = false;

        [SerializeField]
        private CinemachineBlendListCamera _virtualBlendlistCamera;

        [SerializeField] 
        private int _defaultPriority = 0;
        
        [SerializeField] 
        private int _activePriority = 10;

        [SerializeField] 
        private UnityEvent _onSetCameraActive;
        
        [SerializeField] 
        private UnityEvent _onSetCameraInactive;

        [SerializeField] 
        private bool _changeVirtualCameraTarget = false;
        
        private Transform _target;

        public Action<Transform> onTargetChanged;
        
        public void SetCameraActive()
        {
            _onSetCameraActive?.Invoke();
            if (_isBlendlistCamera == true && _virtualBlendlistCamera != null)
            {
                _virtualBlendlistCamera.m_Priority = _activePriority;
            }
            else
            {
                _virtualCamera.m_Priority = _activePriority;
            }
        }

        public void SetCameraInactive()
        {
            _onSetCameraInactive?.Invoke();
            if (_isBlendlistCamera == true && _virtualBlendlistCamera != null)
            {
                _virtualBlendlistCamera.m_Priority = _defaultPriority;
            }
            else
            {
                _virtualCamera.m_Priority = _defaultPriority;
            }
        }

        public void SetTarget(Transform _target)
        {
            this._target = _target;
            if (_changeVirtualCameraTarget)
            {
                if (_isBlendlistCamera == true && _virtualBlendlistCamera != null)
                {
                    _virtualBlendlistCamera.m_Follow = _target;
                    _virtualBlendlistCamera.m_LookAt = _target;
                }
                else
                {
                    _virtualCamera.m_Follow = _target;
                    _virtualCamera.m_LookAt = _target;
                }
            }
            
            onTargetChanged?.Invoke(_target);
        }

        public void WrapCamera(Vector3 _positionDelta)
        {
            if (_isBlendlistCamera)
            {
                _virtualBlendlistCamera.OnTargetObjectWarped(_virtualCamera.Follow, _positionDelta);
            }

            else
            {
                _virtualCamera.OnTargetObjectWarped(_virtualCamera.Follow, _positionDelta);
            }
        }

        public Transform Target => _target;
    }
}