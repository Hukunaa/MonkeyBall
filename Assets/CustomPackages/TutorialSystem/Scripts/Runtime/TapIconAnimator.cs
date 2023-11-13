using DG.Tweening;
using UnityEngine;

namespace TutorialSystem.Scripts.Runtime
{
    public class TapIconAnimator : MonoBehaviour
    {
        [SerializeField] 
        private Transform _imageTransform;

        [SerializeField] 
        private float _startLocalPosition;
        
        [SerializeField] 
        private float _endLocalPosition = 10;
        
        [SerializeField] 
        private float _translateDuration = 2;
       
        [SerializeField] 
        private float _waitDuration = 1;

        [SerializeField] private LoopType _loopType;
        
        private Sequence _animationSequence;
        
        private void OnEnable()
        {
            _animationSequence = DOTween.Sequence();
            _animationSequence
                .SetUpdate(true)
                .Append(_imageTransform.DOLocalMoveY(_startLocalPosition, 0))
                .Append(_imageTransform.DOLocalMoveY(_endLocalPosition, _translateDuration))
                .AppendInterval(_waitDuration)
                .SetLoops(-1, _loopType);
        }

        private void OnDisable()
        {
            _animationSequence.Kill();
        }
    }
}