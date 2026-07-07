using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button SoundButton;
    [SerializeField] private TextMeshProUGUI SoundButtonText;
    [SerializeField] private Button MusicButton;
    [SerializeField] private TextMeshProUGUI MusicButtonText;
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
        SoundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSound();
            SoundButtonText.text = $"Sound: {SoundManager.Instance.getSoundVolume()}";
        });
        MusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            MusicButtonText.text = $"Music: {MusicManager.Instance.GetMusicVolume()}";
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        
        SoundButtonText.text = $"Sound: {SoundManager.Instance.getSoundVolume()}";
        MusicButtonText.text = $"Music: {MusicManager.Instance.GetMusicVolume()}";
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameResumed -= GameManager_OnGameResumed;
            GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        }
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
