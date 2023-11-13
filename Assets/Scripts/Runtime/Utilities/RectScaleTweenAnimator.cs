using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    [RequireComponent(typeof(RectTransform))]
    public class RectScaleTweenAnimator : MonoBehaviour
    {
        [SerializeField] 
        private float _scaleDuration;

        private RectTransform _rect;

        [SerializeField] 
        private UnityEvent _onScaleUpAnimationStart;
        
        [SerializeField] 
        private UnityEvent _onScaleUpAnimationEnd;
        
        [SerializeField]
        private UnityEvent _onScaleDownAnimationStart;
        
        [SerializeField] 
        private UnityEvent _onScaleDownAnimationEnd;

        private void GetRect()
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
        }
        
        public void PlayScaleUpAnimation()
        {
            GetRect();
            StartCoroutine(PlayScaleUpAnimationCoroutine());
        }
        
        public void PlayScaleDownAnimation()
        {
            GetRect();
            StartCoroutine(PlayScaleDownAnimationCoroutine());
        }

        private IEnumerator PlayScaleUpAnimationCoroutine()
        {
            _rect.localScale = Vector3.zero;
            
            _onScaleUpAnimationStart?.Invoke();
            var tween = _rect.DOScale(1, _scaleDuration);
            yield return tween.WaitForCompletion();
            _onScaleUpAnimationEnd?.Invoke();
        }
        
        private IEnumerator PlayScaleDownAnimationCoroutine()
        {
            _rect.localScale = Vector3.one;
            
            _onScaleDownAnimationStart?.Invoke();
            
            var tween = _rect.DOScale(0, _scaleDuration);
            yield return tween.WaitForCompletion();
            _onScaleDownAnimationEnd?.Invoke();
        }
    }
}