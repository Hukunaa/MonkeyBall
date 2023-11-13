using GameplayManagers;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay.Character
{
    public class GameStateEventDispatcher : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<VoidEventChannel> _startNewRoundEventChannelAssetRef;

        public UnityEvent _onNewRoundStart;
        
        public UnityEvent _onPrepareNewAttempt;
        
        public UnityEvent _onAttemptPreparationEnd;
        
        public UnityEvent _onNewAttemptStart;

        public UnityEvent _onGameplayEnd;
        
        public UnityEvent _onAttemptComplete;
        
        private AttemptManager _attemptManager;
        private AsyncOperationHandle<VoidEventChannel> _startNewRoundEventChannelLoadHandle;

        private void Awake()
        {
            _attemptManager = FindObjectOfType<AttemptManager>();
        }
        
        private void OnEnable()
        {
            _attemptManager._onAttemptPreparationStart.AddListener(OnAttemptPreparationStart);
            _attemptManager._onAttemptPreparationEnd.AddListener(OnAttemptPreparationEnd);
            _attemptManager._onAttemptStart.AddListener(OnAttemptStart);
            _attemptManager._onAttemptEnd.AddListener(OnAttemptEnd);
            _attemptManager._onAttemptComplete.AddListener(OnAttemptComplete);
            
            _startNewRoundEventChannelLoadHandle = _startNewRoundEventChannelAssetRef.LoadAssetAsync<VoidEventChannel>();
            _startNewRoundEventChannelLoadHandle.Completed += _handle =>
            {
                _handle.Result.onEventRaised += OnNewRoundStart;
            };
        }
        
        private void OnDisable()
        {
            _attemptManager._onAttemptPreparationStart.RemoveListener(OnAttemptPreparationStart);
            _attemptManager._onAttemptPreparationEnd.RemoveListener(OnAttemptPreparationEnd);
            _attemptManager._onAttemptStart.RemoveListener(OnAttemptStart);
            _attemptManager._onAttemptEnd.RemoveListener(OnAttemptEnd);
            _attemptManager._onAttemptComplete.RemoveListener(OnAttemptComplete);

            _startNewRoundEventChannelLoadHandle.Result.onEventRaised -= OnNewRoundStart;
            Addressables.Release(_startNewRoundEventChannelLoadHandle);
        }

        private void OnAttemptStart()
        {
            _onNewAttemptStart?.Invoke();
        }

        private void OnAttemptPreparationStart()
        {
            _onPrepareNewAttempt?.Invoke();
        }
        
        private void OnAttemptPreparationEnd()
        {
            _onAttemptPreparationEnd?.Invoke();
        }
        
        private void OnAttemptEnd()
        {
            _onGameplayEnd?.Invoke();
        }

        private void OnAttemptComplete()
        {
            _onAttemptComplete?.Invoke();
        }

        private void OnNewRoundStart()
        {
            _onNewRoundStart?.Invoke();
        }
    }
}
