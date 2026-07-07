using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private float timeElapsed = 0f;
    private bool isTimerActive = false;
    private Lander boundLander;
    private GameInput boundInput;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    public static GameManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private List<GameLevel> gameLevels;
    [SerializeField] private string gameOverSceneName = "GameOverScene";
    private static int currentLevel = 1;
    private int totalScore = 0;
    private float totalTimeElapsed = 0f;

    private void Start()
    {
        InitializeLevel();
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    private void Update()
    {
        if (isTimerActive)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            totalScore = 0;
            totalTimeElapsed = 0f;
            currentLevel = 1;
        }

        StartCoroutine(InitializeLevelNextFrame());
    }

    private IEnumerator InitializeLevelNextFrame()
    {
        yield return null;
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        score = 0;
        timeElapsed = 0f;
        isTimerActive = false;

        ResolveSceneReferences();
        BindLander();
        BindInput();
        LoadCurrentLevel();
    }

    private void BindInput()
    {
        if (boundInput != null)
        {
            boundInput.OnMenuPressed -= Game_OnMenuPressed;
        }

        boundInput = GameInput.Instance;

        if (boundInput != null)
        {
            boundInput.OnMenuPressed += Game_OnMenuPressed;
        }
    }

    private void Game_OnMenuPressed(object sender, System.EventArgs e)
    {
        PauseUnPauseGame();
    }

    public void PauseUnPauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void ResolveSceneReferences()
    {
        cinemachineCamera = UnityEngine.Object.FindFirstObjectByType<CinemachineCamera>();
    }

    private void BindLander()
    {
        if (boundLander != null)
        {
            boundLander.OnCoinPickup -= Lander_OnCoinPickup;
            boundLander.OnLanded -= Lander_OnLanded;
            boundLander.OnStateChanged -= Lander_OnStateChanged;
        }

        boundLander = Lander.Instance;

        if (boundLander != null)
        {
            boundLander.OnCoinPickup += Lander_OnCoinPickup;
            boundLander.OnLanded += Lander_OnLanded;
            boundLander.OnStateChanged += Lander_OnStateChanged;
        }
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        AddScore(e.Score);
    }
    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        int scoreAddition = 100;
        AddScore(scoreAddition);
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        if (e.state == Lander.State.Normal)
        {
            isTimerActive = true;

            if (boundLander != null && cinemachineCamera != null)
            {
                cinemachineCamera.Target.TrackingTarget = boundLander.transform;
            }

            if (CinemachineCameraZoom.Instance != null)
            {
                CinemachineCameraZoom.Instance.SetOrthographicSizeToNormal();
            }
        }
        else if (e.state == Lander.State.GameOver)
        {
            isTimerActive = false;
        }
    }

    private void LoadCurrentLevel()
    {
        if (boundLander == null)
        {
            //Debug.LogWarning("Lander instance not found when loading level.");
            return;
        }

        bool levelLoaded = false;

        foreach(GameLevel level in gameLevels)
        {
            if(level.LevelNumber == currentLevel)
            {
                GameLevel spawnedLevel = Instantiate(level, Vector3.zero, Quaternion.identity);
                boundLander.transform.position = spawnedLevel.GetLanderPosition();

                if (cinemachineCamera != null)
                {
                    cinemachineCamera.Target.TrackingTarget = spawnedLevel.GetCameraStartTarget();
                }

                if (CinemachineCameraZoom.Instance != null)
                {
                    CinemachineCameraZoom.Instance.SetOrthographicSize(spawnedLevel.GetZoomOutSize());
                }

                levelLoaded = true;
                break;
            }
        }

        if (!levelLoaded)
        {
            Debug.LogWarning($"No GameLevel found for level {currentLevel}.");
        }
    }

    public void LoadNextLevel()
    {
        int nextLevel = currentLevel + 1;

        foreach (GameLevel level in gameLevels)
        {
            if (level.LevelNumber == nextLevel)
            {
                // accumulate this level's score/time into totals before advancing
                totalScore += score;
                totalTimeElapsed += timeElapsed;

                currentLevel = nextLevel;
                SceneManager.LoadScene(0);
                return;
            }
        }

        // No next level: accumulate this level then go to Game Over
        totalScore += score;
        totalTimeElapsed += timeElapsed;

        //Debug.LogWarning($"No next GameLevel found for level {nextLevel}. Loading Game Over scene '{gameOverSceneName}'.");
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public float GetTotalTime()
    {
        return totalTimeElapsed;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }
}
