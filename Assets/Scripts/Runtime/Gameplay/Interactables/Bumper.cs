using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gameplay.Player;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    private float _bumperForce;
    private Vector3 _startScale;

    private void Start()
    {
        _startScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 _dir = new Vector3(other.transform.position.x, 0, other.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            other.gameObject.GetComponentInParent<GliderMovement>().AdditionalForces += _dir.normalized * _bumperForce;
            transform.DOKill();
            transform.DOScale(transform.localScale * 1.2f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => { transform.DOScale(transform.localScale * 0.8f, 0.1f).SetEase(Ease.InBack); });
        }
    }
}
