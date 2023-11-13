using UnityEngine;

namespace UI.GameplayUI
{
    public class HeightUI : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _parentRectTransform;

        [SerializeField] 
        private RectTransform _cursorRectTransform;

        [SerializeField] 
        private SlicedFilledImage _bar;
        
        [SerializeField] 
        private Transform _target;

        [SerializeField] 
        private int _maxHeight = 200;
    
        // Update is called once per frame
        void Update()
        {
            if (_target == null)
            {
                return;
            }
            
            var cursorYPos = _target.position.y / _maxHeight;
            _cursorRectTransform.anchoredPosition = new Vector2(_cursorRectTransform.anchoredPosition.x,
                _parentRectTransform.rect.height * cursorYPos);
            _bar.fillAmount = cursorYPos;
        }

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }
    }
}
