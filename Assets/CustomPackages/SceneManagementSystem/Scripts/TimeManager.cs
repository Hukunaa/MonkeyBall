using System;
using DG.Tweening;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;

namespace SceneManagementSystem.Scripts
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private BoolEventChannel _changeTimePausedEvent;
        // [SerializeField] private ChangeTimeScaleEventChannel updateTimeScaleEventChannel;

        private bool _gamePaused;
	
        private float _timeScaleBeforePause;
        private float _fixedDeltaTimeBeforePause;

        private void OnEnable()
        {
            _changeTimePausedEvent.onEventRaised += ChangeTimePause;
            // updateTimeScaleEventChannel.OnEventRaised += ChangeTimeScale;
        }

        private void OnDisable()
        {
            _changeTimePausedEvent.onEventRaised -= ChangeTimePause;
            // updateTimeScaleEventChannel.OnEventRaised -= ChangeTimeScale;
        }

        private void ChangeTimePause(bool timePaused)
        {
            if (timePaused)
            {
                if (_gamePaused)
                    return;

                _gamePaused = true;
                _timeScaleBeforePause = Time.timeScale;
                _fixedDeltaTimeBeforePause = Time.fixedDeltaTime;
                Time.timeScale = 0;
            }

            else
            {
                if (!_gamePaused)
                    return;

                Time.timeScale = _timeScaleBeforePause;
                Time.fixedDeltaTime = _fixedDeltaTimeBeforePause;
                _gamePaused = false;
            }
        }

        private void ChangeTimeScale(float newScale, bool lerp, float lerpTime)
        {
            if (Math.Abs(Time.timeScale - newScale) < .01) return;
			
            if (lerp)
            {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, newScale, lerpTime);
                DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, Time.fixedUnscaledDeltaTime * newScale, lerpTime);
            }

            else
            {
                Time.timeScale = newScale;
                Time.fixedDeltaTime *= newScale;
            }
        }
    }
}