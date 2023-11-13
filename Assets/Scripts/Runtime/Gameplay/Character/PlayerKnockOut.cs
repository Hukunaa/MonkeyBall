using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Character
{
    public class PlayerKnockOut : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent _onKnockOutPhaseStart;

        [SerializeField] 
        private UnityEvent _onPlayerKnockedOut;
        
        [SerializeField] 
        private float _knockOutForce;
        
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void MoveToStand(Vector3 _standPosition)
        {
            transform.SetPositionAndRotation(_standPosition, quaternion.identity);
            _onKnockOutPhaseStart?.Invoke();
        }
        
        public void KnockOutPlayer()
        {
            _onPlayerKnockedOut?.Invoke();
            _rb.AddForce(Vector3.forward * _knockOutForce);
        }
    }
}