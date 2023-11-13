using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ButtonClickedEffect : MonoBehaviour
{
    [SerializeField] 
    private float _scaleAmount;

    [SerializeField] 
    private float _effectDuration;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void PlayEffect()
    {
        Sequence effectSequence = DOTween.Sequence();
        effectSequence.Append(_rectTransform.DOScale(_scaleAmount, _effectDuration));
        effectSequence.Append(_rectTransform.DOScale(1, _effectDuration));
    }
}
