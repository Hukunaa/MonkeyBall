using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Character;

public class EmojiTaunt : MonoBehaviour
{

    private Player _player;

    [SerializeField]
    private ParticleSystem _emoji01;
    [SerializeField]
    private ParticleSystem _emoji02;
    [SerializeField]
    private ParticleSystem _emoji03;
    [SerializeField]
    private ParticleSystem _emoji04;
    [SerializeField]
    private ParticleSystem _emoji05;

    public void EmojiTauntPlay(int emoji)
    {
    switch (emoji)
        {
            case 1:
                if (_emoji01 != null)
                {
                    _emoji01.Play();
                } 
                break;

            case 2:
                if (_emoji02 != null)
                {
                    _emoji02.Play();
                }
                break;

            case 3:
                if (_emoji03 != null)
                {
                    _emoji03.Play();
                }
                break;

            case 4:
                if (_emoji04 != null)
                {
                    _emoji04.Play();
                }
                break;

            case 5:
                if (_emoji05 != null)
                {
                    _emoji05.Play();
                }
                break;

            default:
                break;
        }
    }

    //Play random emoji (for AI)
    public void EmojiTauntRandom()
    {
        int randomCase = Random.Range(1, 10);
        EmojiTauntPlay(randomCase);
    }

    public void EmojiTauntRandomGood()
    {
        int randomCase = Random.Range(4, 7);
        EmojiTauntPlay(randomCase);
    }

    public void EmojiTauntRandomBad()
    {
        int randomCase = Random.Range(0, 4);
        EmojiTauntPlay(randomCase);
    }
}
