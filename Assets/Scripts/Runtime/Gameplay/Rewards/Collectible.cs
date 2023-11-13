using Gameplay.Character;
using Gameplay.Rewards;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private int _points;
    [SerializeField]
    private SphereCollider _trigger;
    [SerializeField]
    private ParticleSystem _collectParticle;
    [SerializeField]
    private GameObject _starParticle;
    [SerializeField]
    private AudioSource _collectAudio;

    private Player _player;

    public bool _pickedUp;

    public void PickedUp(Player player)
    {
        _pickedUp = true;
        this._player = player; 
        _player.Score.AddScore(_points);
        _collectParticle.Play();
        _starParticle.SetActive(false);
        _collectAudio.Play();
        _trigger.enabled = false;
    }

    public void ResetPickup()
    {
        _pickedUp = false;
        _starParticle.SetActive(true);
        _trigger.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player info = other.GetComponentInParent<Player>();    // Get collectible

            if (info != null)
            {
                PickedUp(info);
            }
        }
    }
}
