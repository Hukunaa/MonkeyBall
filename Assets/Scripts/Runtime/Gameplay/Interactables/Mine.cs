using Gameplay.Character;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    private float _explosionForce;
    [SerializeField]
    private float _explosionRadius;
    [SerializeField]
    private ParticleSystem _explosionSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<GliderMovement>().enabled)
            {
                other.GetComponentInParent<TargetPracticeCharacterController>().ActiveWreckingBall();
            }

            other.gameObject.GetComponentInParent<Rigidbody>().AddExplosionForce(_explosionForce * 10, transform.position, _explosionRadius);
            _explosionSystem.Emit(100);
            Destroy(this.gameObject, 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, _explosionRadius);
    }
}
