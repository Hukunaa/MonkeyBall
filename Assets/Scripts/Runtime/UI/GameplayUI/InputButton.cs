using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayUI
{
    public class InputButton : MonoBehaviour
    {
        [SerializeField]
        private Image _actionIcon;
        [SerializeField]
        private Image _buttonBackground;
        [SerializeField]
        private Shadow _buttonShadow;
        [SerializeField]
        private Color _buttonColor0;
        [SerializeField]
        private Color _buttonShadow0;
        [SerializeField]
        private Color _buttonColor1;
        [SerializeField]
        private Color _buttonShadow1;

        [SerializeField] 
        private Sprite[] _buttonSprites;
        
        public void ChangeActionIcon(int _spriteIndex)
        {
            _actionIcon.sprite = _buttonSprites[_spriteIndex];
            if (_spriteIndex == 0)
            {
                _actionIcon.color = _buttonShadow0;
                _buttonBackground.color = _buttonColor0;
                _buttonShadow.effectColor = _buttonShadow0;
            }
            else if (_spriteIndex == 1)
            {
                _actionIcon.color = _buttonShadow1;
                _buttonBackground.color = _buttonColor1;
                _buttonShadow.effectColor = _buttonShadow1;
            }
        }

        public void ChangeVisibility(bool _visible)
        {
            _actionIcon.enabled = _visible;
            _buttonBackground.enabled = _visible;
            _buttonShadow.enabled = _visible;
        }
    }
}
