using GeneralScriptableObjects.EventChannels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {

        [SerializeField]
        private Animator _playerAnimator;
        [SerializeField]
        private Animator _bagAnimator;
        [SerializeField]
        private Transform _playerAnimatorParent;
        [SerializeField]
        private Transform _playerBall;
        [SerializeField]
        private float _animatorSpeedOnRoll;
        [SerializeField]
        private float _transitionSpeed;
        [SerializeField]
        private float _angularVelocityThresholdBeforeFalling;
        [SerializeField]
        private AnimationStateEventChannel _onAnimationStateChanged;
        [SerializeField]
        private float _fallingRotationMultiplier = 1.0f;

        private Rigidbody _playerRigidbody;
        [SerializeField]
        private ANIMATION_STATE _animationState;

        private const string PlayerIdle = "Idle";
        private const string PlayerFly = "Fly";
        private const string PlayerRun = "Run";
        private const string PlayerFalling = "Falling";
        
        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
            _playerBall.localScale = Vector3.zero;
        }

        public void ResetAnimation()
        {
            ChangeAnimationState(ANIMATION_STATE.IDLE);
        }
        
        public void ChangeAnimationState(ANIMATION_STATE _state)
        {
            if (_state != _animationState)
            {
                _animationState = _state;
                SwapAnimation();
                _onAnimationStateChanged.RaiseEvent(_state);
            }
        }

        private void Update()
        {
            /*if (_animationState == ANIMATION_STATE.ROLLING || _animationState == ANIMATION_STATE.FALLING)
            {
                _playerAnimator.speed = _animatorSpeedOnRoll * _playerRigidbody.velocity.magnitude;
            }
            else
                _playerAnimator.speed = 1;*/

            if (_playerRigidbody.angularVelocity.magnitude > _angularVelocityThresholdBeforeFalling && _animationState == ANIMATION_STATE.ROLLING)
                ChangeAnimationState(ANIMATION_STATE.FALLING);

            if(_animationState == ANIMATION_STATE.FALLING)
            {
                _playerAnimatorParent.Rotate(_playerRigidbody.angularVelocity * _fallingRotationMultiplier * Time.deltaTime);
                _playerAnimator.speed = 1;
            }
            else if(_animationState == ANIMATION_STATE.ROLLING)
            {
                _playerAnimatorParent.forward = _playerRigidbody.velocity.magnitude > 0.01 ? _playerRigidbody.velocity.normalized : transform.forward;
                _playerAnimator.speed = _animatorSpeedOnRoll * _playerRigidbody.angularVelocity.magnitude;
            }
            else if(_animationState == ANIMATION_STATE.FLYING)
            {
                _playerAnimatorParent.transform.rotation = _playerAnimatorParent.parent.rotation;
                _playerAnimator.speed = 1;
            }
            else 
            {
                _playerAnimator.speed = 1;
            }

        }

        private void SwapAnimation()
        {
            switch (_animationState)
            {
                case ANIMATION_STATE.ROLLING:
                    Roll();
                    break;
                case ANIMATION_STATE.FLYING:
                    Fly();
                    break;
                case ANIMATION_STATE.IDLE:
                    Idle();
                    break;
                case ANIMATION_STATE.FALLING:
                    Fall();
                    break;
            }
        }

        public void OpenBag()
        {
            if (_bagAnimator != null)
                _bagAnimator.SetBool("Open", true);
        }

        public void CloseBag()
        {
            if (_bagAnimator != null)
                _bagAnimator.SetBool("Open", false);
        }

        void Fly()
        {
            _playerBall.DOKill();
            _playerBall.DOScale(0, _transitionSpeed).SetEase(Ease.InBack);
            _playerAnimator.CrossFade(PlayerFly, _transitionSpeed);
            OpenBag();
        }

        void Roll()
        {
            _playerBall.localScale = Vector3.zero;
            _playerBall.DOKill();
            _playerBall.DOScale(0.5f, _transitionSpeed).SetEase(Ease.OutBack);
            _playerAnimator.CrossFade(PlayerRun, _transitionSpeed);

            CloseBag();
        }

        void Idle()
        {
            _playerBall.DOKill();
            _playerBall.DOScale(0.0f, _transitionSpeed).SetEase(Ease.OutBack);
            _playerAnimator.CrossFade(PlayerIdle, _transitionSpeed);
            CloseBag();
        }

        void Fall()
        {
            _playerAnimator.CrossFade(PlayerFalling, _transitionSpeed);
            CloseBag();
        }

        public ANIMATION_STATE AnimationState { get => _animationState; }
        public Animator BagAnimator { get => _bagAnimator; set => _bagAnimator = value; }
    }
}
