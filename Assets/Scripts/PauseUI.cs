using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button MainMenuButton;

    private void Awake()
    {
        PauseButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Resume();
        });
        MainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        gameObject.SetActive(false);
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

}
