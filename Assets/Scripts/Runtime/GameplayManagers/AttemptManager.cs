using System.Collections;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using UI.GameplayUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

namespace GameplayManagers
{
    public class AttemptManager : MonoBehaviour
    {
        [SerializeField] 
        private float _delayBeforeStart = 2;
        
        [Header("Broadcasting on")]
        [SerializeField] 
        private AssetReferenceT<VoidEventChannel>  _onAttemptPreparationStartAssetRef;
        
        [SerializeField] 
        private AssetReferenceT<VoidEventChannel> _onAttemptStartEventChannelAssetRef;
        
        [SerializeField]
        private AssetReferenceT<VoidEventChannel> _onAttemptCompleteEventChannelAssetRef;

        [SerializeField]
        public IntVariable StartRoundTextAttemptCount;

        [SerializeField]
        private IntVariable StartRoundTextMaxAttemptCount;

        [SerializeField]
        private IntVariable StartRoundTextRoundCount;

        [SerializeField] 
        private BoolVariable _isAttemptInProgress;
        
        [SerializeField] 
        private TMPAnimator _readyGoUIText;

        [SerializeField]
        private TMPAnimator _attemptInfoText;

        [SerializeField]
        private TextMeshProUGUI _attemptCountText;

        [SerializeField]
        private TextMeshProUGUI _roundCountText;

        [SerializeField]
        private AudioSource _countdownSound3;

        [SerializeField]
        private AudioSource _countdownSound2;

        [SerializeField]
        private AudioSource _countdownSound1;

        [SerializeField] 
        private float _delayBeforeAttemptComplete;
        
        [Header("Events")]
        public UnityEvent _onAttemptPreparationStart;
        public UnityEvent _onAttemptPreparationEnd;
        public UnityEvent _onAttemptStart;
        public UnityEvent _onAttemptEnd;
        public UnityEvent _onAttemptComplete;

        private AsyncOperationHandle<VoidEventChannel> _attemptPreparationLoadHandle;
        private VoidEventChannel _onAttemptPreparationStartEventChannel;
        
        private AsyncOperationHandle<VoidEventChannel> _attemptStartLoadHandle;
        private VoidEventChannel _onAttemptStartEventChannel;
        
        private AsyncOperationHandle<VoidEventChannel> _attemptEndLoadHandle;
        private VoidEventChannel _onAttemptEndEventChannel;

        private void Awake()
        {
            _attemptPreparationLoadHandle = _onAttemptPreparationStartAssetRef.LoadAssetAsync<VoidEventChannel>();
            _attemptPreparationLoadHandle.Completed +=
                _handle => _onAttemptPreparationStartEventChannel = _handle.Result;
            
            _attemptStartLoadHandle = _onAttemptStartEventChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _attemptStartLoadHandle.Completed +=
                _handle => _onAttemptStartEventChannel = _handle.Result;
            
            _attemptEndLoadHandle = _onAttemptCompleteEventChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _attemptEndLoadHandle.Completed +=
                _handle => _onAttemptEndEventChannel = _handle.Result;
        }

        private void OnDestroy()
        {
            Addressables.Release(_attemptPreparationLoadHandle);
            Addressables.Release(_attemptStartLoadHandle);
            Addressables.Release(_attemptEndLoadHandle);
        }

        public void StartNextAttempt()
        {
            StartCoroutine(PrepareNewAttemptCoroutine());
        }

        private IEnumerator PrepareNewAttemptCoroutine()
        {
            if (_attemptPreparationLoadHandle.IsDone == false || _attemptStartLoadHandle.IsDone == false ||
                _attemptEndLoadHandle.IsDone == false)
            {
                yield return null;
            }
            
            _onAttemptPreparationStart?.Invoke();
            _onAttemptPreparationStartEventChannel.RaiseEvent();
            
            yield return new WaitForSeconds(_delayBeforeStart);
            _onAttemptPreparationEnd?.Invoke();
            
            StartCoroutine(StartAttempt());
        }
        
        private IEnumerator StartAttempt()
        {
            _isAttemptInProgress.SetValue(true);

            if (_attemptInfoText != null)
            {
                _roundCountText.text = "Round " + StartRoundTextRoundCount.Value.ToString();
                yield return StartCoroutine(_attemptInfoText.ShowTextForDuration("Attempt " + StartRoundTextAttemptCount.Value.ToString() + "/" + StartRoundTextMaxAttemptCount.Value.ToString(), 1.5f));
            } 
            yield return new WaitForSeconds(.5f);
            StartCoroutine(_readyGoUIText.ShowTextForDuration("3", 0f));
            _countdownSound3.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(_readyGoUIText.ShowTextForDuration("2", 0f));
            _countdownSound2.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(_readyGoUIText.ShowTextForDuration("1", 0f));
            _countdownSound1.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(_readyGoUIText.ShowTextForDuration("GO", .5f));
            
            _onAttemptStart?.Invoke();
            _onAttemptStartEventChannel.RaiseEvent();
        }
        
        public void AttemptComplete()
        {
            StartCoroutine(AttemptCompleteCoroutine());
        }

        private IEnumerator AttemptCompleteCoroutine()
        {
            _isAttemptInProgress.SetValue(false);
            _onAttemptEnd?.Invoke();
            _onAttemptEndEventChannel.RaiseEvent();
            yield return new WaitForSeconds(_delayBeforeAttemptComplete);
            _onAttemptComplete?.Invoke();
        }
    }
}
