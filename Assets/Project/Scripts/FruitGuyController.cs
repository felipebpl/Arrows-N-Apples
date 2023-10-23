using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGuyController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _applePrefab;
    [SerializeField] private Rigidbody2D _bananaPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _body;

    private Rigidbody2D _fruit;

    public void Die()
    {
        _fruit.isKinematic = false;
        _fruit.transform.parent = null;

        _animator.SetTrigger("Die");
        AudioManager.Instance.Play("GuyGrunt");
    }

    public void StickArrowToBody(Transform arrow)
    {
        arrow.parent = _body;
    }

    public void SetFruit(FruitType fruitType)
    {
        _fruit = fruitType switch
        {
            FruitType.Apple => Instantiate(_applePrefab, _body),
            FruitType.Banana => Instantiate(_bananaPrefab, _body),
            _ => throw new ArgumentOutOfRangeException(nameof(fruitType), fruitType, null)
        };
    }
}
