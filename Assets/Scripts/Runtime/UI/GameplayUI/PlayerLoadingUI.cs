using GeneralScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class PlayerLoadingUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _waitingForPlayersTmp;
        
        [SerializeField] 
        private IntVariable _playersCount;

        [SerializeField] 
        private IntVariable _playersLoadedCount;

        [SerializeField]
        private string _playersLoadingCompleteMessage = "Loading complete";
        
        [SerializeField] 
        private UnityEvent _onAllPlayersLoaded;
        
        private void OnEnable()
        {
            _playersLoadedCount.onValueChanged += UpdateUI;
        }

        private void OnDisable()
        {
            _playersLoadedCount.onValueChanged -= UpdateUI;
        }

        public void UpdateUI()
        {
            if (IsPlayersLoadingComplete())
            {
                _waitingForPlayersTmp.text = _playersLoadingCompleteMessage;
                _onAllPlayersLoaded?.Invoke();
                return;
            }
            
            _waitingForPlayersTmp.text = $"{_playersLoadedCount.Value}/{_playersCount.Value}";
        }

        private bool IsPlayersLoadingComplete()
        {
            return _playersLoadedCount.Value == _playersCount.Value;
        }
    }
}
