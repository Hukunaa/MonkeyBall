using System;
using System.Collections;
using SceneManagementSystem.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNameSetter : MonoBehaviour
{
    [SerializeField] private UnityEvent<string> _setPlayerName;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        while (GameManager.Instance == null || GameManager.Instance.DataLoaded == false)
        {
            yield return null;
        }
        
        _setPlayerName?.Invoke(GameManager.Instance.PlayerDataContainer.PlayerName);
    }
}
