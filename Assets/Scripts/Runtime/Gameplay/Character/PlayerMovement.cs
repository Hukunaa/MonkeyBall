using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Player
{
    public abstract class PlayerMovement : MonoBehaviour
    {
        [SerializeField] 
        protected float _mass = .5f;

        [SerializeField] 
        protected float _drag = 0.01f;

        [SerializeField] 
        protected float _angularDrag = 0;

        [SerializeField] 
        private UnityEvent _onMovementEnabled;
        
        protected Rigidbody _rb;
        protected TargetPracticeCharacterController TargetPracticeCharacterController;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            TargetPracticeCharacterController = GetComponent<TargetPracticeCharacterController>();
        }

        protected virtual void OnEnable()
        {
            _rb.mass = _mass;
            _rb.drag = _drag;
            _rb.angularDrag = _angularDrag;
            _onMovementEnabled?.Invoke();
        }
    }
}