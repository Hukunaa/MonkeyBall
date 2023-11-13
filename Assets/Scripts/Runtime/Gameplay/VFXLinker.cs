using System.Collections;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class VFXLinker : MonoBehaviour
    {
        [SerializeField] 
        private bool _deactivateVFX;
        
        //BALL EVENTS HERE
        private UnityEvent _ballBurstTrigger;
        private UnityEvent _ballTrailOnTrigger;
        private UnityEvent _ballTrailOffTrigger;

        //GLIDER EVENTS HERE
        private UnityEvent _wingsBurstTrigger;
        private UnityEvent _wingsTrailOnTrigger;
        private UnityEvent _wingsTrailOffTrigger;

        private PlayerSkinHandler _skinHandler;

        private void Start()
        {
            _skinHandler = GetComponent<PlayerSkinHandler>();
            _skinHandler.OnBallSkinUpdated += SetupBallVFXTrigger;
            _skinHandler.OnGliderSkinUpdated += SetupGliderVFXTrigger;

        }
        private void OnDestroy()
        {
            _skinHandler.OnBallSkinUpdated -= SetupBallVFXTrigger;
            _skinHandler.OnGliderSkinUpdated -= SetupGliderVFXTrigger;
        }

        public void SetupBallVFXTrigger()
        {
            var ballVFXTrigger = GetComponentInChildren<BallVFXTrigger>();
            _ballBurstTrigger = ballVFXTrigger.BallBurstFX;
            _ballTrailOnTrigger = ballVFXTrigger.BallTrailOnFX;
            _ballTrailOffTrigger = ballVFXTrigger.BallTrailOffFX;
        }

        public void SetupGliderVFXTrigger()
        {
            var gliderVFXTrigger = GetComponentInChildren<GliderVFXTrigger>();
            _wingsBurstTrigger = gliderVFXTrigger.WingsBurstFX;
            _wingsTrailOnTrigger = gliderVFXTrigger.WingsTrailOnFX;
            _wingsTrailOffTrigger = gliderVFXTrigger.WingsTrailOffFX;
        }

        public void TriggerVFX(int type)
        {
            if (_deactivateVFX) return;
            
            switch (type)
            {
                case 1:
                    _ballBurstTrigger.Invoke();
                    break;
                case 2:
                    _ballTrailOnTrigger.Invoke();
                    break;
                case 3:
                    _ballTrailOffTrigger.Invoke();
                    break;
                case 4:
//                    _wingsBurstTrigger.Invoke();
                    break;
                case 5:
                    _wingsTrailOnTrigger.Invoke();
                    break;
                case 6:
                    _wingsTrailOffTrigger.Invoke();
                    break;
                default:
                    Debug.LogError("Invalid trigger type: " + type);
                    break;
            }
        }

        public void TriggerBallTrailForDuration(float _duration)
        {
            StartCoroutine(TriggerBallTrailForDurationCoroutine(_duration));
        }

        private IEnumerator TriggerBallTrailForDurationCoroutine(float _duration)
        {
            TriggerVFX(2);
            yield return new WaitForSeconds(_duration);
            TriggerVFX(3);
        }
    }
}
