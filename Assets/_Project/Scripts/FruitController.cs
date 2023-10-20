using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FruitController : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPsPrefab;

    public void Explode()
    {
        Instantiate(_explosionPsPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
