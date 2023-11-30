using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public event Action OnGamePaused;
    public event Action OnGameUnpaused;

    private bool _isPaused;
    private bool _canPause = true;

    private void OnEnable()
    {
        ArrowController.OnArrowReachedDestionation += DisablePause;
    }

    private void OnDisable()
    {
        ArrowController.OnArrowReachedDestionation -= DisablePause;
    }

    public void TogglePause()
    {
        if (_isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!_canPause)
        {
            return;
        }

        Time.timeScale = 0f;
        _isPaused = true;

        OnGamePaused?.Invoke();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        OnGameUnpaused?.Invoke();
    }

    private void DisablePause()
    {
        _canPause = false;
    }
}
