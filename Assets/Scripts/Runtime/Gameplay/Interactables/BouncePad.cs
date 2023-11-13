using System;
using CustomUtilities;
using Gameplay.Character;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] 
    private int _bounceStrength;

    [SerializeField][Range(0, 90)]
    private int _verticalProjectionAngle = 45;

    [SerializeField] private ETriggerType _triggerType = ETriggerType.Collision;
    
    private enum ETriggerType {Collision, Trigger}

    private Vector3 GetBounceDirection()
    {
        var forward = transform.forward;
        float rad = Mathf.Deg2Rad * _verticalProjectionAngle;
        return new Vector3(forward.x * Mathf.Cos(rad), Mathf.Sin(rad), forward.z * Mathf.Cos(rad));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggerType == ETriggerType.Collision || other.CompareTag("Player") == false) return;
       
        var rb = other.GetComponent<Rigidbody>();
        ProjectRigidBody(rb);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_triggerType == ETriggerType.Trigger || collision.gameObject.CompareTag("Player") == false) return;
        
        var rb = collision.gameObject.GetComponent<Rigidbody>();
        ProjectRigidBody(rb);
    }

    private void ProjectRigidBody(Rigidbody _rb)
    {
        _rb.GetComponent<TargetPracticeCharacterController>().ActiveWreckingBall();
        _rb.GetComponent<PlayerBoost>().ApplyBoost(GetBounceDirection() * _bounceStrength);
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;
        Gizmos.DrawLine(position, position + GetBounceDirection() * 15);
    }
}
