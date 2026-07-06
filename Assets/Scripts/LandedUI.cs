using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private TextMeshProUGUI nextButtonText;
    [SerializeField] private Button nextButton;

    private Action nextButtonAction;

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonAction?.Invoke( );
        });
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        Show();
        switch (e.landingType)
        {
            case Lander.landingType.Success:
                titleText.text = "Successful Landing!";
                nextButtonText.text = "Next Level";
                nextButtonAction = GameManager.Instance.LoadNextLevel;
                break;
            case Lander.landingType.WrongLadingArea:
                titleText.text = "Crash Landing!";
                nextButtonText.text = "Restart Level";
                nextButtonAction = GameManager.Instance.RestartLevel;
                break;
            case Lander.landingType.SteepAngle:
                titleText.text = "Steep Angle!";
                nextButtonText.text = "Restart Level";
                nextButtonAction = GameManager.Instance.RestartLevel;
                break;
            case Lander.landingType.TooFast:
                titleText.text = "Too Fast!";
                nextButtonText.text = "Restart Level";
                nextButtonAction = GameManager.Instance.RestartLevel;
                break;
        }
            float timeElapsed = Mathf.Round(GameManager.Instance.GetTimeElapsed());
        dataText.text = $"{Mathf.Round(e.Score)}\n{Mathf.Round(e.landingSpeed)}\n{Mathf.Abs(Mathf.Round(e.dotVector * 100f))}\n{timeElapsed}s";
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
