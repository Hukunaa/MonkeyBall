using System;
using Gameplay.Player;
using ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] 
        private bool _isPlayer;
        
        [SerializeField] 
        private string _playerName;
        
        [SerializeField] 
        private AssetReferenceT<PlayerInfoEventChannel> _onPlayerTurnCompleteChannelAssetRef;

        public UnityAction onPlayerNameChanged;

        private Rigidbody _rb;
        private PlayerAttemptManager _playerAttemptManager;
        private PlayerKnockOut _playerKnockOut;
        private KnockOutStand _knockOutStand;
        private PlayerSkinHandler _playerSkinHandler;
        private ITimer _timer;
        private IScore _score;
        
        private AsyncOperationHandle<PlayerInfoEventChannel> _playerTurnCompleteChannelLoadHandle;
        private PlayerInfoEventChannel _onPlayerTurnCompleteEventChannel;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerAttemptManager = GetComponent<PlayerAttemptManager>();
            _playerKnockOut = GetComponent<PlayerKnockOut>();
            _playerSkinHandler = GetComponent<PlayerSkinHandler>();
            _timer = GetComponent<ITimer>();
            _score = GetComponent<IScore>();
        }
        
        private void OnEnable()
        {
            _playerTurnCompleteChannelLoadHandle = _onPlayerTurnCompleteChannelAssetRef.LoadAssetAsync<PlayerInfoEventChannel>();
            _playerTurnCompleteChannelLoadHandle.Completed += _handle =>
            {
                _onPlayerTurnCompleteEventChannel = _handle.Result;
            };
        }

        private void OnDisable()
        {
            Addressables.Release(_playerTurnCompleteChannelLoadHandle);
        }

        public void NotifyPlayerTurnComplete()
        {
            _onPlayerTurnCompleteEventChannel.RaiseEvent(this);
        }

        public void SetPlayerName(string _name)
        {
            _playerName = _name;
            onPlayerNameChanged?.Invoke();
        }
        
        public void KnockOut()
        {
            gameObject.SetActive(false);
        }
        
        public Rigidbody Rb => _rb;

        public PlayerAttemptManager PlayerAttemptManager => _playerAttemptManager;

        public PlayerKnockOut PlayerKnockOut => _playerKnockOut;

        public PlayerSkinHandler PlayerSkinHandler => _playerSkinHandler;
        public ITimer Timer => _timer;
        public IScore Score => _score;
        
        public KnockOutStand KnockOutStand
        {
            get => _knockOutStand;
            set => _knockOutStand = value;
        }

        public bool IsPlayer => _isPlayer;
        
        public string PlayerName => _playerName;
    }
}
