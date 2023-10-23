using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitRope : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _applePrefab;
    [SerializeField] private Rigidbody2D _bananaPrefab;
    [SerializeField] private Transform _attachmentPoint;

    private Rigidbody2D _fruit;

    public void SetFruit(FruitType fruitType)
    {
        _fruit = fruitType switch
        {
            FruitType.Apple => Instantiate(_applePrefab, _attachmentPoint),
            FruitType.Banana => Instantiate(_bananaPrefab, _attachmentPoint),
            _ => throw new ArgumentOutOfRangeException(nameof(fruitType), fruitType, null)
        };
    }
}
