using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class DataInitializer : MonoBehaviour
    {
        [SerializeField] 
        private bool _useTinySauce;
        
        [SerializeField] 
        private UnityEvent _onDataCopied;

        private Action<bool, bool> _grdpAction;
        
        private void Awake()
        {
            if (_useTinySauce)
            {
                _grdpAction = delegate(bool a, bool b) { Initialize(); };
                TinySauce.SubscribeToConsentGiven(_grdpAction);
            }

            else
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            if (_useTinySauce)
            {
                TinySauce.UnsubscribeToConsentGiven(_grdpAction);
            }
        }

        private void Initialize()
        {
            DataLoader.CopyFromResourcesToPersistent();
            _onDataCopied?.Invoke();
        }
    }
}
