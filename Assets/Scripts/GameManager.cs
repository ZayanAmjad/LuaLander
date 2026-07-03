using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private float timeElapsed = 0f;
    private bool isTimerActive = false;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
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
        }
        else if (e.state == Lander.State.GameOver)
        {
            isTimerActive = false;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
}
