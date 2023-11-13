using System;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Gameplay.Player
{
    public class WreckingBallMovement : PlayerMovement
    {
        [SerializeField]
        private UnityEvent _onCollision;

        [SerializeField]
        private float _gravityMultiplier = 1;

        [SerializeField]
        private float collisionRepulsionForce;

        [SerializeField]
        private float sandStopTime;

        private bool waitedSandStopTime;

        private TargetPracticeCharacterController _controller;

        private void Update()
        {
            _rb.AddForce(Physics.gravity * _gravityMultiplier);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_controller == null)
                _controller = GetComponent<TargetPracticeCharacterController>();

            if (collision.collider.CompareTag("Player") && _controller.CharacterState == TargetPracticeCharacterController.ECharacterStates.ROLLING)
            {
                _onCollision.Invoke();
            }

            if (_controller.CharacterState == TargetPracticeCharacterController.ECharacterStates.WRECKING)
            {
                _onCollision.Invoke();
                waitedSandStopTime = false;

                if (collision.collider.CompareTag("Player"))
                {
                    collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce((collision.collider.transform.position - transform.position).normalized * collisionRepulsionForce, ForceMode.Impulse);
                }

                if (collision.collider.sharedMaterial != null && collision.collider.sharedMaterial.name == "Sand")
                {
                    StartCoroutine(StopSphereWithDelay(sandStopTime));
                }
            }

        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.sharedMaterial != null && collision.collider.sharedMaterial.name == "Sand" && waitedSandStopTime == true)
            {
                Rigidbody sphereRB = _controller.gameObject.GetComponent<Rigidbody>();
                sphereRB.velocity = Vector3.zero;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.sharedMaterial != null && collision.collider.sharedMaterial.name == "Sand")
            {
                waitedSandStopTime = false;
            }
        }

        private IEnumerator StopSphereWithDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            waitedSandStopTime = true;
        }
    }
}