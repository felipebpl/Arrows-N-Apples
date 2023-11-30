using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public static event Action OnArrowReachedDestionation;
    public static event Action OnShotMissed;
    public static event Action OnShotHitApple;
    public static event Action OnShotHitGuy;

    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private LayerMask _solidLayerMask;
    [SerializeField] private LayerMask _missLayerMask;

    private bool _wasShot;
    private bool _isInMovement;
    private Vector3 _direction;
    private bool _hasHitSomething;

    public void Fire(float force)
    {
        transform.parent = null;
        _direction = transform.up * force;

        _rb2d.isKinematic = false;
        _rb2d.AddForce(_direction, ForceMode2D.Impulse);

        _wasShot = true;
        _isInMovement = true;
    }

    private void Update()
    {
        if (!_wasShot || !_isInMovement)
        {
            return;
        }

        Vector2 velocity = _rb2d.velocity;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasHitSomething)
        {
            return;
        }

        //if hit apple
        if (other.gameObject.TryGetComponent(out FruitController fruit))
        {
            _hasHitSomething = true;
            fruit.Explode();

            AudioManager.Instance.Play("AppleHit");

            OnArrowReachedDestionation?.Invoke();
            OnShotHitApple?.Invoke();
            return;
        }

        //if hit guy
        if (other.gameObject.TryGetComponent(out FruitGuyController appleGuy))
        {
            _hasHitSomething = true;
            StopMoving();

            appleGuy.Die();
            appleGuy.StickArrowToBody(transform);
            AudioManager.Instance.Play("GuyHit");

            OnArrowReachedDestionation?.Invoke();

            DOVirtual.DelayedCall(1f, () => OnShotHitGuy?.Invoke());
            return;
        }

        //if hit ground or wall
        if (((1 << other.gameObject.layer) & _solidLayerMask) != 0)
        {
            _hasHitSomething = true;

            StopMoving();

            OnArrowReachedDestionation?.Invoke();
            OnShotMissed?.Invoke();
        }

        //if missed everything
        if (((1 << other.gameObject.layer) & _missLayerMask) != 0)
        {
            _hasHitSomething = true;

            OnArrowReachedDestionation?.Invoke();
            OnShotMissed?.Invoke();
        }
    }

    private void StopMoving()
    {
        _rb2d.velocity = Vector2.zero;
        _rb2d.isKinematic = true;
        _isInMovement = false;
    }
}
