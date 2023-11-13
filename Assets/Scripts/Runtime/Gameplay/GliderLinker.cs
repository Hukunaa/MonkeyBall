using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderLinker : MonoBehaviour
{
    private PlayerAnimator _animator;
    void Start()
    {
        _animator = GetComponentInParent<PlayerAnimator>();
        _animator.BagAnimator = GetComponent<Animator>();
        GetComponent<Animator>().SetBool("Open", false);
    }
}
