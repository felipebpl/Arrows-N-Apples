using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelView : MonoBehaviour
{
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _quitButton;

    private LevelManager _levelManager;

    private void OnEnable()
    {
        _pauseSystem.OnGamePaused += OpenPausePanel;
        _pauseSystem.OnGameUnpaused += ClosePausePanel;
    }

    private void OnDisable()
    {
        _pauseSystem.OnGamePaused -= OpenPausePanel;
        _pauseSystem.OnGameUnpaused -= ClosePausePanel;

        _quitButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        _quitButton.onClick.AddListener(_pauseSystem.UnpauseGame);
        _quitButton.onClick.AddListener(_levelManager.ReturnToMenu);
    }

    private void OpenPausePanel()
    {
        _pausePanel.SetActive(true);
    }

    private void ClosePausePanel()
    {
        _pausePanel.SetActive(false);
    }
}
