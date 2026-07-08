using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject DownArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private Image fuelBar;

    private bool controlsVisible = true;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed -= GameManager_OnGameResumed;
        }
    }

    private void Update()
    {
        if (!controlsVisible || GameManager.Instance == null)
        {
            return;
        }

        UpArrow.SetActive(Lander.Instance.GetSpeedY() > 0);
        DownArrow.SetActive(Lander.Instance.GetSpeedY() < 0);
        LeftArrow.SetActive(Lander.Instance.GetSpeedX() < 0);
        RightArrow.SetActive(Lander.Instance.GetSpeedX() > 0);

        fuelBar.fillAmount = Lander.Instance.GetFuel();

        statsText.text = 
            GameManager.Instance.GetCurrentLevel().ToString() + '\n' +
            GameManager.Instance.GetScore().ToString() + '\n' +
            Mathf.Round(GameManager.Instance.GetTimeElapsed()).ToString() + "s" + '\n' +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX())).ToString() + '\n' +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY())).ToString() + '\n';
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        controlsVisible = false;
    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        controlsVisible = true;
    }
}
