using GameplayManagers;
using GeneralScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class AttemptUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _currentRoundText;

        [SerializeField] 
        private IntVariable _attemptCount;

        [SerializeField] 
        private IntVariable _currentLayoutMaxAttempts;
        
        private void Awake()
        {
            _attemptCount.onValueChanged += UpdateCurrentRoundText;
        }
        
        private void UpdateCurrentRoundText()
        {
            _currentRoundText.text = $"{_attemptCount.Value}/{_currentLayoutMaxAttempts.Value}" ;
        }
    }
}
