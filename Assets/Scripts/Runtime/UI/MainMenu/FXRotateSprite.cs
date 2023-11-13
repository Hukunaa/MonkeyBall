using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FXRotateSprite : MonoBehaviour
{
    [SerializeField]
    private float _degrees;
    [SerializeField]
    private float _speed;

    void Start()
    {
        GetComponent<Transform>().DORotate(new Vector3(0, 0, _degrees), _speed, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutBack);
    }
}
