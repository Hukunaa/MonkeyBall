using GameplayManagers;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.Settings;
using UnityEngine;

namespace Gameplay.Character.AI
{
    [RequireComponent(typeof(TargetPracticeCharacterController))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] 
        private AITargetPracticeSettings _settings;
        
        [SerializeField] 
        private VoidEventChannel _onRoundStart;

        [SerializeField] 
        private VoidEventChannel _stopAIEventChannel;

        private TargetPracticeCharacterController targetPracticeCharacterController;
        private PlayerBoost _playerBoost;

        private AIStateMachine _aiStateMachine;

        private Rigidbody _rigidbody;

        [SerializeField]
        private TargetManager.ETargetType _currentTargetType;
        
        [SerializeField]
        private GameObject _currentTarget;
        
        private bool _roundStarted = false;
        
        private void Awake()
        {
            targetPracticeCharacterController = GetComponent<TargetPracticeCharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerBoost = GetComponent<PlayerBoost>();
            
            _onRoundStart.onEventRaised += StartRound;
            _stopAIEventChannel.onEventRaised += DisableAI;
            
            _aiStateMachine = new AIStateMachine(this);
            _aiStateMachine.Initialize(_aiStateMachine.IdleState);
        }

        private void OnDestroy()
        {
            _onRoundStart.onEventRaised -= StartRound;
            _stopAIEventChannel.onEventRaised -= DisableAI;
        }

        private void Update()
        {
            if (_aiStateMachine == null) return;
            _aiStateMachine.Update();
        }

        private void StartRound()
        {
            _roundStarted = true;
        }

        public void SetTarget(TargetManager.ETargetType targetType, GameObject target)
        {
            _currentTargetType = targetType;
            _currentTarget = target;
        }
        
        public void ResetStateMachine()
        {
            _aiStateMachine?.Initialize(_aiStateMachine.IdleState);
        }

        private void DisableAI()
        {
            _aiStateMachine = null;
        }
        
        public bool RoundStarted => _roundStarted;

        public AIStateMachine AIStateMachine => _aiStateMachine;

        public TargetPracticeCharacterController TargetPracticeCharacterController => targetPracticeCharacterController;

        public PlayerBoost PlayerBoost => _playerBoost;

        public Rigidbody Rigidbody => _rigidbody;

        public TargetManager.ETargetType CurrentTargetType => _currentTargetType;
        public GameObject CurrentTarget => _currentTarget;


        public AITargetPracticeSettings Settings
        {
            get => _settings;
            set => _settings = value;
        }
    }
}