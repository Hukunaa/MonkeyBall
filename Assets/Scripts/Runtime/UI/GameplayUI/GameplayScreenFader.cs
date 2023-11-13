using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayScreenFader : MonoBehaviour
{
    [SerializeField]
    private Image _block1Left;

    [SerializeField]
    private Image _block1Right;

    [SerializeField]
    private Image _block2Centre;

    [SerializeField]
    private Image _block2CentreLeft;

    [SerializeField]
    private Image _block2CentreRight;

    [SerializeField]
    private Image _block2Left;

    [SerializeField]
    private Image _block2Right;

    [SerializeField] 
    private float _fillDuration;

    [SerializeField] private UnityEvent _onFadeInStart;
    [SerializeField] private UnityEvent _onFadeOutComplete;

    [ContextMenu("Fade in")]
    public void FadeIn()
    {
        _onFadeInStart?.Invoke();
        StartCoroutine(FadeInCoroutine());
    }
    
    [ContextMenu("Fade out")]
    public void FadeOut(float _delay = 0)
    {
        StartCoroutine(FadeOutCoroutine(_delay));
    }

    private IEnumerator FadeInCoroutine()
    {
        _block1Left.fillOrigin = (int)Image.Origin90.TopLeft;
        _block1Left.DOFillAmount(0.5f, _fillDuration);
        _block1Right.fillOrigin = (int)Image.Origin90.BottomRight;
        _block1Right.DOFillAmount(0.5f, _fillDuration);

        yield return new WaitForSeconds(_fillDuration);
        _block2Centre.fillOrigin = (int)Image.OriginVertical.Bottom;
        _block2Centre.DOFillAmount(1f, _fillDuration);
        _block2Left.fillOrigin = (int)Image.OriginVertical.Top;
        _block2Left.DOFillAmount(1f, _fillDuration);
        _block2Right.fillOrigin = (int)Image.OriginVertical.Top;
        _block2Right.DOFillAmount(1f, _fillDuration);

        yield return new WaitForSeconds(_fillDuration);
        _block2CentreLeft.DOFillAmount(1f, _fillDuration);
        _block2CentreRight.DOFillAmount(1f, _fillDuration);
    }

    private IEnumerator FadeOutCoroutine(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _block2Centre.fillOrigin = (int)Image.OriginVertical.Top;
        _block2Centre.DOFillAmount(0f, _fillDuration);
        _block2Left.fillOrigin = (int)Image.OriginVertical.Bottom;
        _block2Left.DOFillAmount(0f, _fillDuration);
        _block2Right.fillOrigin = (int)Image.OriginVertical.Bottom;
        _block2Right.DOFillAmount(0f, _fillDuration);

        yield return new WaitForSeconds(_fillDuration);
        _block1Left.fillOrigin = (int)Image.Origin90.TopRight;
        _block1Left.DOFillAmount(0f, _fillDuration);
        _block1Right.fillOrigin = (int)Image.Origin90.BottomLeft;
        _block1Right.DOFillAmount(0f, _fillDuration);

        yield return new WaitForSeconds(_fillDuration);
        _block2CentreLeft.fillAmount = 0f;
        _block2CentreRight.fillAmount = 0f;

        _onFadeOutComplete?.Invoke();
    }
}
