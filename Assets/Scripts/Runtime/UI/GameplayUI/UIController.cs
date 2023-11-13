using UnityEngine;

namespace UI.GameplayUI
{
    public class UIController : MonoBehaviour
    {
        private UIManager _uiManager;

        private void Awake()
        {
            _uiManager = FindObjectOfType<UIManager>();
        }

        public void SetGameplayUIVisible(bool _visible)
        {
            _uiManager.SetGameplayUIVisibility(_visible);
        }
    
        public void SetHeaderUIVisible(bool _visible)
        {
            _uiManager.SetHeaderUIVisibility(_visible);
        }
    
        public void SetPlayerTurnCompleteUIVisible(bool _visible)
        {
            _uiManager.SetPlayerTurnCompleteUIVisibility(_visible);
        }
    
        public void SetAttemptFeedbackUIVisible(bool _visible)
        {
            _uiManager.SetAttemptFeedbackUIVisibility(_visible);
        }

        public void SetJoystickVisibility(bool _visible)
        {
            _uiManager.SetJoystickVisibility(_visible);
        }
        
        public void ChangeTransformButtonIcon(int _iconIndex)
        {
            _uiManager.SetTransformButtonIcon(_iconIndex);
        }

        public void SetTransformButtonVisible(bool _visible)
        {
            _uiManager.SetTransformButtonVisibility(_visible);
        }

        public void OnTransformButtonClicked()
        {
            _uiManager.OnTransformButtonClicked();
        }

        public void SetRunButtonVisibility(bool _visible)
        {
            _uiManager.SetRunButtonVisibility(_visible);
        }

        public void OnRunButtonClicked()
        {
            _uiManager.OnRunButtonClicked();
        }
    }
}
