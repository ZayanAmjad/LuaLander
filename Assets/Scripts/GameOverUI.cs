using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private TextMeshProUGUI ScoreText;
    private void Awake()
    {
        MainMenuButton.onClick.AddListener(()=>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            int finalScore = GameManager.Instance.GetTotalScore();
            int totalSeconds = Mathf.RoundToInt(GameManager.Instance.GetTotalTime());
            ScoreText.text = "Final Score: " + finalScore.ToString() + "\nTotal Time: " + totalSeconds.ToString() + "s";
        }
    }
}
