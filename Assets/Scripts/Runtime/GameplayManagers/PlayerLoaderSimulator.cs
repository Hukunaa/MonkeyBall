using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using GeneralScriptableObjects;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameplayManagers
{
    public class PlayerLoaderSimulator : MonoBehaviour
    {
        [SerializeField] 
        private VoidEventChannel _startIntroductionEventChannel;

        [SerializeField] 
        private Vector2 _delayRangeBetweenPlayerLoad;

        [SerializeField] 
        private Vector2Int _playersAlreadyLoadedRange;
        
        [SerializeField] 
        private IntVariable _playersLoadedCount;

        [SerializeField][Tooltip("Time to wait before to trigger the OnPlayerLoadingEnd event triggers after all players have been loaded.")]
        private float _endDelay;
        
        [SerializeField] 
        private bool _skip;
        
        [FormerlySerializedAs("_onIntroductionStart")] [SerializeField] 
        private UnityEvent _onPlayerLoadingStart;
        
        [FormerlySerializedAs("_onIntroductionEnd")] [SerializeField] 
        private UnityEvent _onPlayerLoadingEnd;
        
        private List<KnockOutStand> _stands;

        private void Awake()
        {
            _startIntroductionEventChannel.onEventRaised += StartIntroduction;
        }

        private void StartIntroduction()
        {
            _startIntroductionEventChannel.onEventRaised -= StartIntroduction;

            if (_skip)
            {
                Debug.Log("Skip Player Loading Simulation");
                _onPlayerLoadingEnd?.Invoke();
                return;
            }
            
            Debug.Log("Start Player Loading Simulation");
            
            Initialize();
            _playersLoadedCount.SetValue(0);
            
            foreach (var knockOutStand in _stands)
            {
                knockOutStand.Hide();
            }
            
            _onPlayerLoadingStart?.Invoke();
            StartCoroutine(IntroductionCoroutine());
        }

        private IEnumerator IntroductionCoroutine()
        {
            var playerLoadedAtStart = Random.Range(_playersAlreadyLoadedRange.x, _playersAlreadyLoadedRange.y);

            var playerStand = _stands.First(x => x.AssignedPlayer.IsPlayer);
            LoadPlayer(playerStand);
            
            for (int i = 0; i < playerLoadedAtStart; i++)
            {
                LoadRandomPlayer();
            }

            while (_stands.Count > 0)
            {
                var delay = Random.Range(_delayRangeBetweenPlayerLoad.x, _delayRangeBetweenPlayerLoad.y);
                yield return new WaitForSeconds(delay);
                LoadRandomPlayer();
            }

            yield return new WaitForSeconds(_endDelay);
            
            Debug.Log("End Player Loading Simulation");

            _onPlayerLoadingEnd?.Invoke();
        }

        private void Initialize()
        {
            _stands = FindObjectsOfType<KnockOutStand>().ToList();
            Debug.Log($"Initialized PlayerLoaderSimulator with {_stands.Count} Stands.");
        }

        private void LoadRandomPlayer()
        {
            var index = Random.Range(0, _stands.Count);
            var stand = _stands[index];
            LoadPlayer(stand);
        }

        private void LoadPlayer(KnockOutStand _stand)
        {
            _stand.MovePlayerToStand();
            _stand.HideUI();
            _stand.Show();
            _stands.Remove(_stand);
            _playersLoadedCount.Increment();
            Debug.Log($"{_stand.AssignedPlayer.gameObject.name} was loaded");
        }
    }
}