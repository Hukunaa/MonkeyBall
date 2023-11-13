using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Character;
using Gameplay.Player;

public class EmojiTauntTrigger : MonoBehaviour
{
    private EmojiTaunt _playerEmoji = null;

    private bool _spamBlock = false;

    [SerializeField]
    private int _spamDelay;

    public void FindPlayerEmoji()
    {
        if (_playerEmoji == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                var Player = obj.GetComponent<Player>();
                if (Player.IsPlayer == true)
                {
                    _playerEmoji = Player.GetComponentInChildren<EmojiTaunt>();
                    break;
                }
            }
        }
    }

    public void TriggerEmojiTaunt(int type)
    {
        if (_spamBlock == false)
        {
            StartCoroutine(EmojiSpamDelay(type));
            _spamBlock = true;
        }
    }

    private IEnumerator EmojiSpamDelay(int type)
    {
        if (_playerEmoji != null)
        {
            _playerEmoji.EmojiTauntPlay(type);
            yield return new WaitForSeconds(_spamDelay);
            _spamBlock = false;
        }
    }
}
