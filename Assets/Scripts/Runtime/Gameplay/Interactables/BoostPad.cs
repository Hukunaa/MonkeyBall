using Gameplay;
using Gameplay.Character;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField]
    private float _boostForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<VFXLinker>().TriggerVFX(2);
            other.gameObject.GetComponentInParent<PlayerBoost>().ApplyBoost(transform.forward * _boostForce);
        }
    }
}
