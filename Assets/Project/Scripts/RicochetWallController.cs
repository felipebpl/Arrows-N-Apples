using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetWallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ArrowController arrow))
        {
            return;
        }

        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        Vector2 ricochetVector = Vector2.Reflect(arrowRb.velocity, Vector2.down);

        arrowRb.velocity = ricochetVector;
    }
}
