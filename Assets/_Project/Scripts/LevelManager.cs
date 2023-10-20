using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public event Action<int> OnLivesUpdated;

    public static LevelManager Instance;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private FruitGuyController _fruitGuyPrefab;
    [SerializeField] private FruitRope _fruitRopePrefab;
    [SerializeField] private GameObject _missWallPrefab;
    [SerializeField] private LevelData[] _levelList;
    [SerializeField] private int _initialLives;

    private int _currentLevel;
    private Camera _mainCamera;

    public int CurrentLives { get; private set; }
    public int InitialLives => _initialLives;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        CurrentLives = InitialLives;
    }

    private void Start()
    {
        AudioManager.Instance.PlaySong("GameSong");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;

        ArrowController.OnShotMissed += HandleShotMissed;
        ArrowController.OnShotHitGuy += HandleShotHitGuy;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;

        ArrowController.OnShotMissed -= HandleShotMissed;
        ArrowController.OnShotHitGuy -= HandleShotHitGuy;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        _currentLevel = 0;
        CurrentLives = _initialLives;

        RetryLevel();
    }

    public void GoToNextLevel()
    {
        _currentLevel++;

        if (_currentLevel >= _levelList.Length)
        {
            //player finished game
            _currentLevel = 0;
        }

        RetryLevel();
    }

    private void HandleSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        LevelData currentLevel = _levelList[_currentLevel];
        float levelDistance = currentLevel.Distance;
        FruitType levelFruit = currentLevel.FruitType;
        
        _mainCamera = Camera.main;
        _mainCamera!.orthographicSize = levelDistance / 3.2f;

        float distanceToTheCenter = levelDistance / 2;

        if (currentLevel.IsRopeLevel)
        {
            FruitRope fruitRope = Instantiate(_fruitRopePrefab,
                new Vector2(distanceToTheCenter * 0.7f, _fruitRopePrefab.transform.position.y), Quaternion.identity);
            fruitRope.SetFruit(levelFruit);
        }
        else
        {
            FruitGuyController fruitGuy = Instantiate(_fruitGuyPrefab, 
                new Vector2(distanceToTheCenter, _fruitGuyPrefab.transform.position.y), Quaternion.identity);
            fruitGuy.SetFruit(levelFruit);
        }

        GameObject player = Instantiate(_playerPrefab,
            new Vector2(-distanceToTheCenter, _playerPrefab.transform.position.y),
            Quaternion.Euler(0, 180, 0));

        Instantiate(_missWallPrefab, new Vector2(distanceToTheCenter + 2, 0), Quaternion.identity);
        FindObjectOfType<BackgroundImage>().SetSprite(currentLevel.BackgroundTexture);
    }

    private void HandleShotMissed()
    {
        CurrentLives--;

        OnLivesUpdated?.Invoke(CurrentLives);
    }

    private void HandleShotHitGuy()
    {
        CurrentLives = 0;

        OnLivesUpdated?.Invoke(CurrentLives);
    }
}
