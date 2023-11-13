using System;
using System.Linq;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTransformFinder : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel _findPlayerEventChannel;

    [SerializeField] 
    private string _playerName;
    
    [SerializeField] 
    private UnityEvent<Transform> _onPlayerTransformFound;
    
    private void Awake()
    {
        _findPlayerEventChannel.onEventRaised += FindPlayer;
    }

    private void OnDestroy()
    {
        _findPlayerEventChannel.onEventRaised -= FindPlayer;
    }

    private void FindPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        var player = players.FirstOrDefault(x => x.name == _playerName);

        if (player == null)
        {
            Debug.Log($"Can't find the player with name {_playerName}.");
            return;
        }
        
        _onPlayerTransformFound?.Invoke(player.transform);
    }
}
