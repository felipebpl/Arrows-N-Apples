using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private HeartView _heartPrefab;
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private Image _powerBar;
    [SerializeField] private CanvasGroup _levelCompletePanel;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _refillLivesButton;
    [SerializeField] private CanvasGroup _missedShotPanel;
    [SerializeField] private CanvasGroup _gameOverPanel;

    private BowController _bowController;
    private List<HeartView> _hearts = new List<HeartView>();

    private void OnEnable()
    {
        ArrowController.OnShotHitApple += ShowLevelCompletePanel;
        ArrowController.OnShotMissed += ShowMissedShotPanel;
        ArrowController.OnShotHitGuy += ShowGameOverPanel;

        LevelManager.Instance.OnLivesUpdated += HandleLivesUpdated;
    }

    private void OnDisable()
    {
        ArrowController.OnShotHitApple -= ShowLevelCompletePanel;
        ArrowController.OnShotMissed -= ShowMissedShotPanel;
        ArrowController.OnShotHitGuy -= ShowGameOverPanel;

        LevelManager.Instance.OnLivesUpdated -= HandleLivesUpdated;
    }

    private void Start()
    {
        _bowController = FindObjectOfType<BowController>();

        for (int i = 0; i < LevelManager.Instance.InitialLives; i++)
        {
            _hearts.Add(Instantiate(_heartPrefab, _heartContainer));

            _hearts[i].SetHeartActive(i <= (LevelManager.Instance.CurrentLives - 1));
        }
    }

    private void Update()
    {
        _powerBar.fillAmount = _bowController.CurrentForce;
    }

    private void ShowLevelCompletePanel()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            _nextLevelButton.onClick.AddListener(LevelManager.Instance.GoToNextLevel);
            _levelCompletePanel.blocksRaycasts = true;
            _levelCompletePanel.DOFade(1, 1);
        });
    }

    private void ShowMissedShotPanel()
    {
        _continueButton.onClick.AddListener(LevelManager.Instance.RetryLevel);
        _missedShotPanel.blocksRaycasts = true;
        _missedShotPanel.DOFade(1, 1);
    }

    private void ShowGameOverPanel()
    {
        _tryAgainButton.onClick.AddListener(LevelManager.Instance.RestartGame);
        _gameOverPanel.blocksRaycasts = true;
        _gameOverPanel.DOFade(1, 1);
    }

    private void HandleLivesUpdated(int lives)
    {
        for (int i = 0; i < LevelManager.Instance.InitialLives; i++)
        {
            _hearts[i].SetHeartActive(i <= (lives - 1));
        }

        _refillLivesButton.gameObject.SetActive(lives <= 0);
    }
}
