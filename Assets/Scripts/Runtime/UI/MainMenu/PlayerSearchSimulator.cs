using System.Collections;
using ScriptableObjects.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSearchSimulator : MonoBehaviour
{
    [SerializeField] 
    TMP_Text _playerSearchTmp;

    [SerializeField] 
    private GameplaySettings _gameplaySettings;

    [SerializeField] 
    private Vector2 _findPlayerTimeRange;

    [SerializeField] 
    private UnityEvent _onSearchSimulationComplete;
    
    private int _playerFoundCount;
    
    public void StartSearchSimulation()
    {
        _playerFoundCount = 1;
        UpdateUI();
        StartCoroutine(SearchSimulationCoroutine());
    }

    private IEnumerator SearchSimulationCoroutine()
    {
        while (_playerFoundCount < _gameplaySettings.PlayerAmount)
        {
            var delay = Random.Range(_findPlayerTimeRange.x, _findPlayerTimeRange.y);
            yield return new WaitForSeconds(delay);
            _playerFoundCount++;
            UpdateUI();
        }
        
        _onSearchSimulationComplete?.Invoke();
    }

    private void UpdateUI()
    {
        _playerSearchTmp.text = $"{_playerFoundCount}/{_gameplaySettings.PlayerAmount}";
    }

    public void CancelSearch()
    {
        StopAllCoroutines();
    }
}
