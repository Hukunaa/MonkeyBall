using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCamera
{
    public class SpiralCamera : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineVirtualCamera _cmCamera;
       
        [SerializeField]
        private float _spiralTimer;

        [SerializeField] 
        private float _startHeight;
        [SerializeField] 
        private float _endHeight;
        [SerializeField] 
        private float _startDistance;
        [SerializeField] 
        private float _endDistance;

        [SerializeField] 
        private bool _skip;

        [SerializeField]
        private UnityEvent _onSpiralStart;
        
        [SerializeField]
        private UnityEvent _onSpiralComplete;
        
        private CinemachineOrbitalTransposer _orbitalTransposer;

        private void Awake()
        {
            _orbitalTransposer = _cmCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
        
        public void PlaySpiralCamera()
        {
            if (_skip)
            {
                _onSpiralComplete?.Invoke();
                return;
            }
            
            _onSpiralStart?.Invoke();
            StartCoroutine(PlaySpiralCoroutine());
        }

        private IEnumerator PlaySpiralCoroutine()
        {
            float currentTimer = 0;
            while (currentTimer < _spiralTimer)
            {
                float t = currentTimer / _spiralTimer;
                var xValue = t * 360;
                var height = Mathf.Lerp(_startHeight, _endHeight, t);
                var distance = Mathf.Lerp(_startDistance, _endDistance, t);
            
                UpdateOrbitalCamera(xValue, height, distance);
                yield return null;
                currentTimer = Mathf.Clamp(currentTimer + Time.deltaTime, 0, _spiralTimer);
            }
            
            _onSpiralComplete?.Invoke();
        }
        
        private void UpdateOrbitalCamera(float _xValue, float _yOffset, float _zOffset)
        {
            _orbitalTransposer.m_XAxis.Value = _xValue;
            _orbitalTransposer.m_FollowOffset = new Vector3(0, _yOffset, -_zOffset);
        }
    }
}