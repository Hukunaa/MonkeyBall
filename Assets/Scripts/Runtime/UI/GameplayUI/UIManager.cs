using GeneralScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] 
        private BoolVariable _isAttemptInProgress;
        
        [SerializeField]
        private UnityEvent<bool> _onSetGameplayUIVisible;
        
        [SerializeField]
        private UnityEvent<bool> _onSetHeaderUIVisible;
        
        [SerializeField]
        private UnityEvent<bool> _onSetPlayerTurnCompleteUIVisible;
        
        [SerializeField]
        private UnityEvent<bool> _onSetAttemptFeedbackUIVisible;
        
        [SerializeField]
        private UnityEvent<bool> _changeJoystickVisibility;
        
        [SerializeField]
        private UnityEvent<int> _changeTransformButtonIcon;
        
        [SerializeField]
        private UnityEvent<bool> _changeTransformButtonVisible;
        
        [SerializeField]
        private UnityEvent _onTransformButtonClicked;
        
        [SerializeField]
        private UnityEvent<bool> _changeRunButtonVisible;
        
        [SerializeField]
        private UnityEvent _onRunButtonClicked;

        public void SetGameplayUIVisibility(bool _visible)
        {
            _onSetGameplayUIVisible?.Invoke(_visible);
        }
        
        public void SetHeaderUIVisibility(bool _visible)
        {
            _onSetHeaderUIVisible?.Invoke(_visible);
        }

        public void SetPlayerTurnCompleteUIVisibility(bool _visible)
        {
            if (_isAttemptInProgress.Value == false) return;
            
            _onSetPlayerTurnCompleteUIVisible?.Invoke(_visible);
        }

        public void SetAttemptFeedbackUIVisibility(bool _visible)
        {
            _onSetAttemptFeedbackUIVisible?.Invoke(_visible);
        }

        public void SetJoystickVisibility(bool _visible)
        {
            _changeJoystickVisibility?.Invoke(_visible);
        }

        public void SetTransformButtonIcon(int _iconIndex)
        {
            _changeTransformButtonIcon?.Invoke(_iconIndex);
        }

        public void SetTransformButtonVisibility(bool _visible)
        {
            _changeTransformButtonVisible?.Invoke(_visible);
        }

        public void OnTransformButtonClicked()
        {
            _onTransformButtonClicked?.Invoke();
        }

        public void SetRunButtonVisibility(bool _visible)
        {
            _changeRunButtonVisible?.Invoke(_visible);
        }

        public void OnRunButtonClicked()
        {
            _onRunButtonClicked?.Invoke();
        }
    }
}
