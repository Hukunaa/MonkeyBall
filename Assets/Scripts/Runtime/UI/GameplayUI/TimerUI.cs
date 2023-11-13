using System;
using GeneralScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _timerTMP;

        [SerializeField] 
        private FloatVariable _attemptTimerVariable;
        
        private void Update()
        {
            UpdateTimer(_attemptTimerVariable.Value);
        }

        private void UpdateTimer(float _time)
        {
            _timerTMP.SetText(TimeSpan.FromSeconds(_time).ToString(@"mm\:ss\:ff"));
        }
    }
}
