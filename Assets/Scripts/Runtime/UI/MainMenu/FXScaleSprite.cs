using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FXScaleSprite : MonoBehaviour
{
    private Vector3 _scale;

    [SerializeField]
    private float frequency;
    [SerializeField]
    private float size;

    private void Start()
    {
        _scale = gameObject.transform.localScale;
        gameObject.transform.DOScale(size, frequency).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
    }

    private void Update()
    {
        
    }
}
