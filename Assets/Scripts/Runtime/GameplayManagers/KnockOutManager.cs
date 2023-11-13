using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.Character;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using Runtime.DataContainers.Player;
using SceneManagementSystem.Scripts;
using ScriptableObjects.DataContainer;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Events;
using PlayerScore = Runtime.DataContainers.Player.PlayerScore;

namespace GameplayManagers
{
    public class KnockOutManager : MonoBehaviour
    {
        [SerializeField] 
        private KnockOutSettings _knockOutSettings;

        [SerializeField] 
        private IntVariable _roundCount;
        
        [SerializeField] 
        private float _delayBeforeKnockOutStart;
        
        [SerializeField] 
        private float _delayAfterRoundInfoDisplay;

        [SerializeField] 
        private float _delayBeforeWinnerCheck = 1;
        
        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;

        [SerializeField] 
        private PlayersRankContainer _playersRankContainer;

        [SerializeField] 
        private VoidEventChannel _onStandsGridInitialized;

        [SerializeField]
        private AudioSource _knockoutAudio;
        
        [Header("Broadcasting on")]
        [SerializeField] 
        private VoidEventChannel _onInitializedEventChannel;

        private List<KnockOutStand> _remainingStands;
        
        public UnityEvent _onWinnerFound;
        public UnityEvent _onNoWinnerFound;
        public UnityEvent _onKnockOutStart;
        public UnityEvent _onKnockOutComplete;

        public UnityAction _onPlayerKnockedOut;

        private int _knockedOutCount;

        private List<KnockOutStand> _standsKnockedOut = new List<KnockOutStand>();

        private void Awake()
        {
            _onStandsGridInitialized.onEventRaised += Initialize;
        }

        private void OnDestroy()
        {
            _onStandsGridInitialized.onEventRaised -= Initialize;
        }

        public void Initialize()
        {
            _remainingStands = FindObjectsOfType<KnockOutStand>().ToList();
            _playersRankContainer.Initialize(_remainingStands.Count);
            _onInitializedEventChannel.RaiseEvent();
        }

        public void Reset()
        {
            _standsKnockedOut.Clear();
            _knockedOutCount = 0;
        }
        
        private void ApplyKnockOut(KnockOutStand _stand, bool _draw)
        {
            Debug.Log($"Knock out {_stand.AssignedPlayer.gameObject.name}.");
            _knockedOutCount++;
            _stand.KnockOutPlayer(_draw);
            _standsKnockedOut.Add(_stand);
            _remainingStands.Remove(_stand);

            if(_knockoutAudio != null)
            {
                _knockoutAudio.Play();
            }
            
            _playersRankContainer.AddPlayerToRanking(_stand.AssignedPlayer);
            _remainingPlayersContainer.RemovePlayer(_stand.AssignedPlayer);
            _onPlayerKnockedOut?.Invoke();
        }
        
        public void StartKnockOutProcess()
        {
            StartCoroutine(KnockOutCoroutine());
        }
        
        private IEnumerator KnockOutCoroutine()
        {
            _onKnockOutStart?.Invoke();
            Reset();
            InitializeRemainingStands();
            
            yield return new WaitForSeconds(_delayBeforeKnockOutStart);
            
            yield return StartCoroutine(IncrementalKnockOut());
            
            QualityRemainingStands();
            
            yield return new WaitForSeconds(_delayAfterRoundInfoDisplay);
            
            foreach (var knockOutStand in _standsKnockedOut)
            {
                knockOutStand.HideUI();
            }
            
            _onKnockOutComplete?.Invoke();

            yield return new WaitForSeconds(_delayBeforeWinnerCheck);
            CheckForWinner();
        }

        private void CheckForWinner()
        {
            if (IsWinnerFound())
            {
                _playersRankContainer.AddPlayerToRanking(_remainingPlayersContainer.RemainingPlayers[0]);
                Player _winner = _remainingPlayersContainer.RemainingPlayers[0];
                PlayerScore _winnerScore = GameManager.Instance.PlayerDataContainer.PlayerScore;

                if (_winner.IsPlayer)
                    _winnerScore.UpdateWins(_winnerScore.Wins + 1);

                _onWinnerFound?.Invoke();
            }

            else
            {
                _onNoWinnerFound?.Invoke();
            }
        }

        private void QualityRemainingStands()
        {
            foreach (var stand in _remainingStands)
            {
                stand.QualifyPlayer();
            }
        }

        private IEnumerator IncrementalKnockOut()
        {
            var roundKnockOutCount = _knockOutSettings.GetRoundKnockoutCount(_roundCount.Value);

            if (roundKnockOutCount == -1)
            {
                Debug.LogError("Exit Knock out phase early.");
                yield break;
            }
            
            while (_knockedOutCount < roundKnockOutCount && _remainingStands.Count > 1)
            {
                List<KnockOutStand> standsComplete = new List<KnockOutStand>();
                
                foreach (var stand in _remainingStands)
                {
                    if (stand.TryReachingScore())
                    {
                        standsComplete.Add(stand);
                    }
                }

                if (_knockedOutCount < roundKnockOutCount && standsComplete.Count > 0)
                {
                    SolveKnockOut(standsComplete);
                }

                yield return null;
            }
            
            foreach (var knockOutStand in _remainingStands)
            {
                knockOutStand.DisplayScore();
            }
        }

        private void SolveKnockOut(List<KnockOutStand> _knockOutCandidate)
        {
            if (_knockOutCandidate.Count == 1)
            {
                ApplyKnockOut(_knockOutCandidate[0], false);
                return;
            }

            var orderedStands = _knockOutCandidate.OrderBy(x => x.AssignedPlayer.Score.CurrentScore).ThenByDescending(x => x.AssignedPlayer.Timer.TimerValue).ToList();

            int stopIndex = Mathf.Clamp(orderedStands.Count - (_knockOutSettings.GetRoundKnockoutCount(_roundCount.Value) - _knockedOutCount), 0, orderedStands.Count) ;
            for (int i = orderedStands.Count - 1; i >= stopIndex; i--)
            {
                var isDraw = orderedStands.Count(x =>
                    x.AssignedPlayer.Score.CurrentScore == orderedStands[i].AssignedPlayer.Score.CurrentScore) > 1;
                ApplyKnockOut(orderedStands[i], isDraw);
                if (_remainingStands.Count == 1) return;
            }
        }
        
        private bool IsWinnerFound()
        {
            return _remainingPlayersContainer.RemainingPlayersCount == 1;
        }

        private void InitializeRemainingStands()
        {
            foreach (var stand in _remainingStands)
            {
                stand.Initialize();
            }
        }
        
        public int RemainingPlayerCount => _remainingPlayersContainer.RemainingPlayersCount;

        public int KnockedOutCount => _knockedOutCount;

        public int RoundKnockoutCount => _knockOutSettings.GetRoundKnockoutCount(_roundCount.Value);
    }
}