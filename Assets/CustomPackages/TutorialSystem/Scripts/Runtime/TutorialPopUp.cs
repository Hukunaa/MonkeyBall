using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TutorialSystem.Scripts.Runtime
{
    public class TutorialPopUp : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _tutorialText;

        [SerializeField] 
        private RectTransform _rectTransform;

        [SerializeField] 
        private Button _nextButton;

        [SerializeField] 
        private VerticalLayoutGroup _layoutGroup;

        [SerializeField] 
        private int _defaultBottomPadding = 32;

        [SerializeField] 
        private int _nextButtonBottomPadding = 64;
        
        [SerializeField]
        private UnityEvent _onNextButtonClicked;
        
        public void DisplayPopUp(string _text, Vector2 _position, bool _showNextButton)
        {
            _nextButton.gameObject.SetActive(_showNextButton);
            _layoutGroup.padding.bottom = _showNextButton ? _nextButtonBottomPadding : _defaultBottomPadding;
            _tutorialText.text = _text;
            _rectTransform.anchoredPosition = _position;
            gameObject.SetActive(true);
        }

        public void HidePopUp()
        {
            gameObject.SetActive(false);
        }
        
        public void OnNextButtonClicked()
        {
            _onNextButtonClicked?.Invoke();
        }

        public Button NextButton => _nextButton;
    }
}