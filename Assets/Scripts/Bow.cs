using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public float speed = 45.0f;  // Speed of the rotation in degrees per second
    public float maxAngle = 75.0f; // Maximum angle from the vertical

    private float angleDirection = 1f;
    private float currentAngle = 0f;

    void Update()
    {
        //if (isStopped) return;

        float rotationChange = speed * Time.deltaTime * angleDirection;
        currentAngle += rotationChange;

        // Flip direction when reaching max angles
        if (Mathf.Abs(currentAngle) >= maxAngle)
        {
            angleDirection *= -1;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void ShootArrow(float strength)
    {
        Rigidbody2D arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, transform.rotation);
        arrowInstance.velocity = transform.up * baseForce * strength;
    }


}