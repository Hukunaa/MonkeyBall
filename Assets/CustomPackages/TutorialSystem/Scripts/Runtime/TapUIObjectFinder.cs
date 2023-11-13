using UnityEngine;

namespace TutorialSystem.Scripts.Runtime
{
    public class TapUIObjectFinder : MonoBehaviour
    {
        [SerializeField] 
        private Camera _mainCamera;

        [SerializeField] 
        private Camera _uiCamera;
        
        [SerializeField] 
        private RectTransform _canvasRectTransform;
        
        private Transform _objectTransform;
        private bool _follow;
        
        private RectTransform _rectTransform;
        private RenderMode _renderMode;
        
        private void Awake()
        {
            _renderMode = transform.root.GetComponent<Canvas>().renderMode;
            _rectTransform = GetComponent<RectTransform>();
        }

        public void PositionOverObjectWithName(string _objectName)
        {
            _objectTransform = GameObject.Find(_objectName).transform;
            _follow = true;
        }

        private void OnDisable()
        {
            _objectTransform = null;
            _follow = false;
        }

        private void Update()
        {
            if (_follow)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_mainCamera, _objectTransform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvasRectTransform, 
                    screenPoint,
                    _renderMode == RenderMode.ScreenSpaceCamera? _uiCamera : null, 
                    out var canvasPos);
                
                _rectTransform.localPosition = canvasPos;
            }
        }
    }
}