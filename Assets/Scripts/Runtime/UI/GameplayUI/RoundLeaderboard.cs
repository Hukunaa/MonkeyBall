using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Character;
using GeneralScriptableObjects;
using ScriptableObjects.DataContainer;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

namespace UI.GameplayUI
{
    public class RoundLeaderboard : MonoBehaviour
    {
        [SerializeField]
        private float _leaderboardDisplayTime = 2;
        
        [SerializeField] 
        private AssetReferenceT<GameObject> _leaderboardEntryAssetRef;

        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;

        [SerializeField] 
        private KnockOutSettings _knockoutOutSettings;

        [SerializeField] 
        private IntVariable _roundCount;

        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField] 
        private Transform _leaderboardEntriesParent;

        [SerializeField] 
        private IntVariable _currentAttempt;
        
        [SerializeField] 
        private IntVariable _layoutAttempts;

        [SerializeField] 
        private UnityEvent _onDisplayLeaderboard;
        
        [SerializeField] 
        private UnityEvent _onHideLeaderboard;
        
        private AsyncOperationHandle<GameObject> _leaderboardEntryLoadHandle;
        private GameObject _leaderboardEntryPrefab;

        private List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

        private void Awake()
        {
            _leaderboardEntryLoadHandle = _leaderboardEntryAssetRef.LoadAssetAsync<GameObject>();
            _leaderboardEntryLoadHandle.Completed += _handle =>
            {
                _leaderboardEntryPrefab = _leaderboardEntryLoadHandle.Result;
            };
        }

        public void CheckLeaderboardShouldDisplay()
        {
            if (_currentAttempt.Value == _layoutAttempts.Value)
            {
                _onHideLeaderboard?.Invoke();
            }
            
            else if (_currentAttempt.Value == 1)
            {
                GenerateRoundEntries();
                Display();
            }

            else
            {
                UpdateRoundEntries();
                Display();
            }
        }

        private void Display()
        {
            _onDisplayLeaderboard?.Invoke();
            _label.text = "Round " + _roundCount.Value.ToString();
            StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_leaderboardDisplayTime);
            _onHideLeaderboard?.Invoke();
        }

        private void GenerateRoundEntries()
        {
            ClearRoundEntries();
            var orderedPlayers = OrderRemainingPlayers();
            for (int i = 0; i < orderedPlayers.Length; i++)
            {
                var entry = CreateLeaderboardEntry();
                UpdateEntry(entry, orderedPlayers[i], i + 1);
                _leaderboardEntries.Add(entry);
            }
        }

        private Player[] OrderRemainingPlayers()
        {
            var orderedPlayers = _remainingPlayersContainer.RemainingPlayers.OrderByDescending(x => x.Score.CurrentScore)
                .ThenBy(x => x.Timer.TimerValue).ToArray();
            return orderedPlayers;
        }

        private void ClearRoundEntries()
        {
            if (_leaderboardEntries.Count == 0) return;
            
            for (int i = _leaderboardEntries.Count - 1; i >= 0; i--)
            {
                Destroy(_leaderboardEntries[i].gameObject);
            }
        
            _leaderboardEntries.Clear();
        }

        private void UpdateRoundEntries()
        {
            var orderedPlayers = OrderRemainingPlayers();

            for (int i = 0; i < orderedPlayers.Length; i++)
            {
                var entry = _leaderboardEntries.First(x => x.PlayerName == orderedPlayers[i].PlayerName);
                UpdateEntry(entry, orderedPlayers[i], i + 1);
                entry.transform.SetSiblingIndex(i);
            }
        }

        private LeaderboardEntry CreateLeaderboardEntry()
        {
            var leaderboardEntry = Instantiate(_leaderboardEntryPrefab, _leaderboardEntriesParent).GetComponent<LeaderboardEntry>();
            return leaderboardEntry;
        }

        private void UpdateEntry(LeaderboardEntry _entry, Player _player, int _rank)
        {
            _entry.InitializeEntry(_rank, _player.PlayerName, _player.Score.CurrentScore, _rank > _remainingPlayersContainer.RemainingPlayersCount - _knockoutOutSettings.GetRoundKnockoutCount(_roundCount.Value), _player.IsPlayer);
        }
    
    }
}
