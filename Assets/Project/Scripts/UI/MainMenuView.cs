using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private GameObject _aboutPanel;

    private void Start()
    {
        AudioManager.Instance.PlaySong("MenuSong");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenAboutPanel()
    {
        _aboutPanel.SetActive(true);
    }

    public void CloseAboutPanel()
    {
        _aboutPanel.SetActive(false);
    }
}
