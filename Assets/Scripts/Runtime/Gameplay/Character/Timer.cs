using UnityEngine;

namespace Gameplay.Character
{
    public class Timer : MonoBehaviour, ITimer
    {
        private bool _timerStarted;
        private float _timerValue;

        private void Update()
        {
            if (_timerStarted == false) return;
            UpdateTimer(_timerValue + Time.deltaTime);
        }
        
        private void UpdateTimer(float _newTimerValue)
        {
            _timerValue = _newTimerValue;
        }

        public void StartTimer()
        {
            _timerStarted = true;
        }

        public void StopTimer()
        {
            _timerStarted = false;
        }

        public void ResetTimer()
        {
            UpdateTimer(0);
        }

        public float TimerValue => _timerValue;
    }
}
