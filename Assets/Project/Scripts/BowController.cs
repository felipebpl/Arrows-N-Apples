using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BowController : MonoBehaviour
{
    private const float AngleOffset = -90;

    [SerializeField] private Transform _playerSpine;
    [SerializeField] private ArrowController _arrow;
    [SerializeField] private LineRenderer _bowLine;
    [SerializeField] private Transform _frontArmSprite;
    [SerializeField] private float _bowRotationSpeed;
    [SerializeField] private Vector2 _minMaxBowAngle;
    [SerializeField] private float _timeToReachMaxForce;
    [SerializeField] private float _arrowBaseVelocity;

    private Camera _mainCamera;
    private Tweener _forceTween;
    private bool _wasAlreadyShot;
    private Vector3[] _linePoints;
    private Vector3[] _originalLinePoints;

    public float CurrentForce { get; private set; }

    private void Start()
    {
        _mainCamera = Camera.main;

        _linePoints = new Vector3[3];
        _bowLine.GetPositions(_linePoints);
        _originalLinePoints = (Vector3[])_linePoints.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wasAlreadyShot)
        {
            return;
        }

        AimBow();

        if (Input.GetMouseButtonDown(0)) //hold mouse button
        {
            _forceTween?.Kill();

            _forceTween = DOVirtual.Float(0, 1, _timeToReachMaxForce, value =>
            {
                _linePoints[1].x = -value * 0.5f;
                _arrow.transform.localPosition = new Vector2(value * 0.25f, 0);
                _frontArmSprite.localScale = new Vector3(1, 1 - (value * 0.35f), 1);
                CurrentForce = value;
            })
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        if (Input.GetMouseButtonUp(0)) //release mouse button
        {
            _forceTween?.Kill();

            ShootArrow();
            ResetPullAnimation();
        }
    }

    private void AimBow()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = mousePosition - _playerSpine.position;
        float angle = AngleOffset + Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, _minMaxBowAngle.x, _minMaxBowAngle.y);

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        _playerSpine.localRotation =
            Quaternion.Lerp(_playerSpine.localRotation, targetRotation, _bowRotationSpeed * Time.deltaTime);

        _bowLine.SetPositions(_linePoints);
    }

    private void ShootArrow()
    {
        _arrow.Fire(_arrowBaseVelocity * CurrentForce);
        AudioManager.Instance.Play("ArrowShot");

        _wasAlreadyShot = true;
    }

    private void ResetPullAnimation()
    {
        _bowLine.SetPositions(_originalLinePoints);
        _frontArmSprite.localScale = new Vector3(1, 1, 1);
    }
}
