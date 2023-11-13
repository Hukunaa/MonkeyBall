using UnityEngine;

namespace UI.GameplayUI
{
    public class UIFollowGameObject : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        [SerializeField] private Transform _target;

        private Camera _cam;
        
        private RectTransform _rectTransform;

        private Vector3 _velocity;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            var pos = _cam.WorldToScreenPoint(_target.position + _offset);
            _rectTransform.position = Vector3.SmoothDamp(_rectTransform.position, pos, ref _velocity, .01f);
        }
    }
}