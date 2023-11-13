using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] 
        private Vector3 _offset;
        
        [SerializeField]
        private Transform _transformToFollow;

        [SerializeField] 
        private UnityEvent _onTargetChanged;

        private void Start()
        {
            if (_transformToFollow != null)
            {
                transform.position = _transformToFollow.position + _offset;
                _onTargetChanged?.Invoke();
            }
        }

        private void Update()
        {
            if (_transformToFollow == null) return;
            
            transform.position = _transformToFollow.position + _offset;
        }

        public void SetTarget(Transform _newTarget)
        {
            _transformToFollow = _newTarget;
            transform.position = _transformToFollow.position + _offset;
            _onTargetChanged?.Invoke();
        }
    }
}