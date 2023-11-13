using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class DelayAction : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent _onDelayOver;

        [SerializeField] 
        private float _delayDuration;
        
        public void StartDelay()
        {
            StartCoroutine(DelayCoroutine());
        }

        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(_delayDuration);
            _onDelayOver?.Invoke();
        }
    }
}