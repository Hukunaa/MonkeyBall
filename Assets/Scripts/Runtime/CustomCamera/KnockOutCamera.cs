using System;
using System.Collections.Generic;
using Cinemachine;
using Gameplay;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;

namespace CustomCamera
{
    public class KnockOutCamera : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineTargetGroup _targetGroup;

        [SerializeField] 
        private VoidEventChannel _onStandGridInitialized;

        private void Awake()
        {
            _onStandGridInitialized.onEventRaised += UpdateTargets;
        }

        private void OnDestroy()
        {
            _onStandGridInitialized.onEventRaised -= UpdateTargets;
        }

        public void UpdateTargets()
        {
            ClearAllTargets();

            var stands = FindObjectsOfType<KnockOutStand>();
            
            foreach (var instance in stands)
            {
                _targetGroup.AddMember(instance.transform, 1, 0);
            }
        }

        private void ClearAllTargets()
        {
            for (int i = _targetGroup.m_Targets.Length - 1; i >= 0; i--)
            {
                _targetGroup.m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
            }
        }
    }
}
