using DG.Tweening;
using UnityEngine;

namespace UI.GameplayUI
{
    public class AnimateElementToScreen : MonoBehaviour
    {
        enum ANIMATION_DIRECTION
        {
            RIGHT, LEFT, DOWN, UP
        }

        [Tooltip("Toogle on if you want the element to be out of screen at the beginning of the animation")]
        [SerializeField]
        private bool _isOffsetAtStart;
        [SerializeField]
        private ANIMATION_DIRECTION _animationDir;
        [SerializeField]
        private Ease _animationEaseType;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationStartDelay;
        [SerializeField]
        private bool _triggerAtStart;

        private RectTransform _rect;
        private Vector2 _startPos;
        private Vector2 _endPos;

        private bool _isActivated;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _startPos = _rect.anchoredPosition;
            if(_isOffsetAtStart)
            {
                switch(_animationDir)
                {
                    case ANIMATION_DIRECTION.RIGHT:
                        _rect.anchoredPosition -= new Vector2(_rect.rect.width, 0);
                        break;
                    case ANIMATION_DIRECTION.LEFT:
                        _rect.anchoredPosition += new Vector2(_rect.rect.width, 0);
                        break;
                    case ANIMATION_DIRECTION.DOWN:
                        _rect.anchoredPosition += new Vector2(0, _rect.rect.height);
                        break;
                    case ANIMATION_DIRECTION.UP:
                        _rect.anchoredPosition -= new Vector2(0, _rect.rect.height);
                        break;
                }
                _endPos = _rect.anchoredPosition;
            }

            if(_triggerAtStart)
                AnimateElement();
        }

        public void AnimateElement()
        {
            AnimateElement(_animationDuration, _animationStartDelay, _animationEaseType);
        }

        public void AnimateElement(float _duration, float delay, Ease _ease)
        {
            Vector2 _pos = _isActivated ? _endPos : _startPos;
            if(_isOffsetAtStart)
            {
                _rect.DOKill();
                _rect.DOAnchorPos(_pos, _duration).SetDelay(delay).SetEase(_ease);
                _isActivated = !_isActivated;
            }
        }
    }
}
