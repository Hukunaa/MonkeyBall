using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Character
{
    public class PlayerBoost : MonoBehaviour
    {
        [SerializeField] 
        private float _lockControlsDuration;

        [SerializeField] private UnityEvent _onLockControlsStart;
        [SerializeField] private UnityEvent _onLockControlsEnd;

        public UnityAction _onPlayerGotBounced;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void ApplyBoost(Vector3 force)
        {
            StopAllCoroutines();
            _rb.AddForce(force, ForceMode.Impulse);
            _onPlayerGotBounced?.Invoke();
            StartCoroutine(LockControlsCoroutine());
        }

        private IEnumerator LockControlsCoroutine()
        {
            _onLockControlsStart?.Invoke();
            yield return new WaitForSeconds(_lockControlsDuration);
            _onLockControlsEnd?.Invoke();
        }
    }
}