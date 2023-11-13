using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PreviewPlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _camHead;
    [SerializeField]
    private Transform _camBody;
    [SerializeField]
    private Transform _camBall;
    [SerializeField]
    private Transform _playerParent;

    [SerializeField]
    private float _transitionDuration;
    [SerializeField]
    private Transform _camera;

    public void MoveToHead()
    {
        _playerParent.DOKill();
        _playerParent.DORotate(new Vector3(0, 0, 0), _transitionDuration).SetEase(Ease.OutBack);
        _camera.DOKill();
        _camera.DOMove(_camHead.position, _transitionDuration).SetEase(Ease.OutBack);
    }

    public void MoveToBody()
    {
        _playerParent.DOKill();
        _playerParent.DORotate(new Vector3(0, 0, 0), _transitionDuration).SetEase(Ease.OutBack);
        _camera.DOKill();
        _camera.DOMove(_camBody.position, _transitionDuration).SetEase(Ease.OutBack);
    }
    public void MoveToBall()
    {
        _playerParent.DOKill();
        _playerParent.DORotate(new Vector3(0, 0, 0), _transitionDuration).SetEase(Ease.OutBack);
        _camera.DOKill();
        _camera.DOMove(_camBall.position, _transitionDuration).SetEase(Ease.OutBack);
    }

    public void MoveToGlider()
    {
        MoveToBall();
        _playerParent.DOKill();
        _playerParent.DORotate(new Vector3(0, 180, 0), _transitionDuration).SetEase(Ease.OutBack);
    }
}
