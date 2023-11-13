using GeneralScriptableObjects;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayManagers
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] 
        private LayoutLoader _layoutLoader;

        [SerializeField]
        private IntVariable _currentLayoutAttempt;

        [SerializeField] 
        private IntVariable _layoutAttempts;
        
        [SerializeField] 
        private IntVariable _currentLayoutMaxAttempts;
        
        [SerializeField] 
        private UnityEvent _onStartNewAttempt;
        
        [SerializeField] 
        private UnityEvent _onLayoutComplete;
        
        private LayoutSO _currentLayout;
        
        public void UpdateAttemptCount()
        {
            _currentLayoutAttempt.SetValue(_currentLayoutAttempt.Value + 1);
        }
        
        public void LoadNewLayout()
        {
            _currentLayout = _layoutLoader.LoadNewLayout();
            _currentLayoutMaxAttempts.SetValue(_currentLayout.Attempts);

            if (_currentLayout == null)
            {
                Debug.Log("No Layout returned by the Layout Loader.");
                return;
            }

            _currentLayoutAttempt.SetValue(0);
            _layoutAttempts.SetValue(_currentLayout.Attempts);
        }

        public void CheckLayoutAttempts()
        {
            if (_currentLayoutAttempt.Value == _currentLayout.Attempts)
            {
                _onLayoutComplete?.Invoke();
            }

            else
            {
                _onStartNewAttempt?.Invoke();
            }
        }

        public LayoutSO CurrentLayout => _currentLayout;
    }
}