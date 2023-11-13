using System;
using System.Collections;
using System.Collections.Generic;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TutorialSystem.Scripts.Runtime
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] 
        private string _tutorialName;
        
        [SerializeField] 
        private GameObject _clickBlocker;
        
        [SerializeField] 
        private TutorialPopUp _tutorialPopup;

        [SerializeField] 
        private GameObject _tutorialFader;
        
        [SerializeField] 
        private BoolEventChannel _timeEventChannel;

        [SerializeField] private ETutorialStartCondition _tutorialStartCondition;
        
        [SerializeField] 
        private VoidEventChannel _startTutorialEventChannel;

        [SerializeField] 
        private Button _startTutorialButton;

        [SerializeField] 
        private bool _requireOtherTutorialCompleted;

        [SerializeField]
        private string _requiredTutorialName;
        
        [SerializeField] 
        private UnityEvent _onTutorialStart;

        [SerializeField] 
        private UnityEvent _onTutorialEnd;
        
        [SerializeField]
        private List<TutorialEntry> _tutorialEntries;
        
        private TutorialEntry _currentEntry;
        
        private int _currentTutorialIndex;

        private enum ETutorialStartCondition
        {
            EventChannel,
            ExternalCall,
            ClickOnAButton
        }
        
        private void Awake()
        {
            RegisterStartTutorialCallback();
        }

        private void RegisterStartTutorialCallback()
        {
            switch (_tutorialStartCondition)
            {
                case ETutorialStartCondition.EventChannel:
                    _startTutorialEventChannel.onEventRaised += StartTutorial;
                    break;
                case ETutorialStartCondition.ExternalCall:
                    break;
                case ETutorialStartCondition.ClickOnAButton:
                    _startTutorialButton.onClick.AddListener(StartTutorial);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            ClearStartTutorialCallback();
        }

        private void ClearStartTutorialCallback()
        {
            switch (_tutorialStartCondition)
            {
                case ETutorialStartCondition.EventChannel:
                    _startTutorialEventChannel.onEventRaised -= StartTutorial;
                    break;
                case ETutorialStartCondition.ExternalCall:
                    break;
                case ETutorialStartCondition.ClickOnAButton:
                    _startTutorialButton.onClick.RemoveListener(StartTutorial);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartTutorial()
        {
            if (TutorialDataManager.IsTutorialComplete(_tutorialName) || 
                (_requireOtherTutorialCompleted && ! TutorialDataManager.IsTutorialComplete(_requiredTutorialName)))
            {
                return;
            }
            
            _onTutorialStart?.Invoke();
            SetTutorialEntryActive();
        }
        
        private void SetTutorialEntryActive()
        {
            StartCoroutine(SetTutorialEntryActiveCoroutine());
        }

        private IEnumerator SetTutorialEntryActiveCoroutine()
        {
            _currentEntry = _tutorialEntries[_currentTutorialIndex];
            
            switch (_currentEntry.AppearCondition)
            {
                case TutorialEntry.ETutorialAppearCondition.Sequence:
                    StartCoroutine(DisplayCurrentEntry());
                    break;
                case TutorialEntry.ETutorialAppearCondition.Delay:
                    yield return new WaitForSeconds(_currentEntry.DelayDuration);
                    StartCoroutine(DisplayCurrentEntry());
                    break;
                case TutorialEntry.ETutorialAppearCondition.Event:
                    _currentEntry.ShowTutorialEventChannel.onEventRaised += ShowTutorialEventChannelTriggered;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowTutorialEventChannelTriggered()
        {
            _currentEntry.ShowTutorialEventChannel.onEventRaised -= ShowTutorialEventChannelTriggered;
            StartCoroutine(DisplayCurrentEntry());
        }

        private IEnumerator DisplayCurrentEntry()
        {
            if (_currentEntry.AppearCondition == TutorialEntry.ETutorialAppearCondition.Event)
            {
                yield return new WaitForSeconds(_currentEntry.ShowDelay);
            }
            
            _currentEntry.OnTutorialEntryStart?.Invoke();
            
            if (_currentEntry.PauseTime)
            {
                _timeEventChannel.RaiseEvent(true);
            }

            if (_currentEntry.UseFadeBackground)
            {
                _tutorialFader.SetActive(true);
            }
            
            _tutorialPopup.DisplayPopUp(_currentEntry.Text, _currentEntry.TutorialWindowPosition, _currentEntry.HideCondition == TutorialEntry.EHideTutorialCondition.NextButton);
            SetHideBehavior(_currentEntry);
        }

        private void CloseEntry()
        {
            StartCoroutine(CloseEntryCoroutine());
        }

        private IEnumerator CloseEntryCoroutine()
        {
            if (_currentEntry.HideCondition == TutorialEntry.EHideTutorialCondition.EventChannel || _currentEntry.HideCondition == TutorialEntry.EHideTutorialCondition.Delay)
            {
                yield return new WaitForSeconds(_currentEntry.HideDelay);
            }
            
            _tutorialPopup.HidePopUp();
            _currentEntry.OnTutorialEntryEnd?.Invoke();
            
            if (_currentEntry.PauseTime)
            {
                _timeEventChannel.RaiseEvent(false);
            }

            if (_currentEntry.UseFadeBackground)
            {
                _tutorialFader.SetActive(false);
            }
            
            CleanBehavior();
            _currentTutorialIndex++;
            
            if (_currentTutorialIndex == _tutorialEntries.Count)
            {
                TutorialComplete();
                yield break;
            }
            
            SetTutorialEntryActive();
        }
        
        private void SetHideBehavior(TutorialEntry _entry)
        {
            switch (_entry.HideCondition)
            {
                case TutorialEntry.EHideTutorialCondition.NextButton:
                    _tutorialPopup.NextButton.onClick.AddListener(CloseEntry);
                    break;
                case TutorialEntry.EHideTutorialCondition.ClickAnyWhere:
                    _clickBlocker.SetActive(true);
                    break;
                case TutorialEntry.EHideTutorialCondition.ClickOnButton:
                    _currentEntry.TrackedHideTutorialButton.onClick.AddListener(CloseEntry);
                    break;
                case TutorialEntry.EHideTutorialCondition.EventChannel:
                    _currentEntry.HideTutorialEventChannel.onEventRaised += CloseEntry;
                    break;
                case TutorialEntry.EHideTutorialCondition.Delay:
                    CloseEntry();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CleanBehavior()
        {
            switch (_currentEntry.HideCondition)
            {
                case TutorialEntry.EHideTutorialCondition.NextButton:
                    _tutorialPopup.NextButton.onClick.RemoveListener(CloseEntry);
                    break;
                
                case TutorialEntry.EHideTutorialCondition.ClickAnyWhere:
                    _clickBlocker.SetActive(false);
                    break;
                
                case TutorialEntry.EHideTutorialCondition.ClickOnButton:
                    _currentEntry.TrackedHideTutorialButton.onClick.RemoveListener(CloseEntry);
                    break;
                
                case TutorialEntry.EHideTutorialCondition.EventChannel:
                    _currentEntry.HideTutorialEventChannel.onEventRaised -= CloseEntry;
                    break;
                case TutorialEntry.EHideTutorialCondition.Delay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnBlockerClicked()
        {
            CloseEntry();
        }

        private void TutorialComplete()
        {
            _tutorialPopup.HidePopUp();
            _onTutorialEnd?.Invoke();
            TutorialDataManager.TutorialComplete(_tutorialName);
            ClearStartTutorialCallback();
            gameObject.SetActive(false);
        }
    }
}