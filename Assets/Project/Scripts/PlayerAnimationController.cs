using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        ArrowController.OnShotHitApple += PlayVictoryAnimation;
    }

    private void OnDisable()
    {
        ArrowController.OnShotHitApple -= PlayVictoryAnimation;
    }

    private void PlayVictoryAnimation()
    {
        _animator.SetTrigger("Victory");
    }
}
