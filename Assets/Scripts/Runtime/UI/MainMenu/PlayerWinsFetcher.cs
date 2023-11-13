using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SceneManagementSystem.Scripts;

public class PlayerWinsFetcher : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    public void FetchWins()
    {
        StartCoroutine("WaitForData");
    }

    private IEnumerator WaitForData()
    {
        while (GameManager.Instance == null || GameManager.Instance.DataLoaded == false)
        {
            yield return null;
        }

        _text.text = GameManager.Instance.PlayerDataContainer.PlayerScore.Wins.ToString();
    }
}
