using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        Show();
        switch (e.landingType)
        {
            case Lander.landingType.Success:
                titleText.text = "Successful Landing!";
                break;
            case Lander.landingType.WrongLadingArea:
                titleText.text = "Crash Landing!";
                break;
            case Lander.landingType.SteepAngle:
                titleText.text = "Steep Angle!";
                break;
            case Lander.landingType.TooFast:
                titleText.text = "Too Fast!";
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
