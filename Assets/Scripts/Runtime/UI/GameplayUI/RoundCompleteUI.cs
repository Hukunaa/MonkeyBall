using System.Collections;
using GameplayManagers;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class RoundCompleteUI : MonoBehaviour
    {
        [SerializeField] 
        private float _delayBeforeShowingUI;

        [SerializeField]
        private float _eliminatePlayerDelay;
        
        [SerializeField] 
        private float _delayBeforeNewRound;

        [SerializeField] 
        private UnityEvent _onShowUI;
        
        [SerializeField] 
        private UnityEvent _onHideUI;

        [Header("Broadcast on")]
        [SerializeField] 
        private VoidEventChannel _onEliminatePlayers;
        
        [SerializeField] 
        private Canvas _canvas;
        

        private AttemptManager _attemptManager;

        private void Awake()
        {
            _attemptManager = FindObjectOfType<AttemptManager>();
            _attemptManager._onAttemptComplete.AddListener(ShowRoundCompleteUI);
        }

        private void OnDestroy()
        {
            _attemptManager._onAttemptComplete.RemoveListener(ShowRoundCompleteUI);
        }

        private void Start()
        {
            Hide();
        }

        public void ShowRoundCompleteUI()
        {
            StartCoroutine(HideAfterDelay());
        }

        public void Hide()
        {
            StopAllCoroutines();
            _canvas.enabled = false;
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_delayBeforeShowingUI);
            _onShowUI?.Invoke();
            _canvas.enabled = true;
            
            yield return new WaitForSeconds(_eliminatePlayerDelay);
            _onEliminatePlayers.RaiseEvent();
            
            yield return new WaitForSeconds(_delayBeforeNewRound);
            Hide();
            _onHideUI?.Invoke();
        }
    }
}
