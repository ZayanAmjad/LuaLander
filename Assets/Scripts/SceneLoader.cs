using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static bool isLoadingScene = false;

    public enum Scene
    {
        MainMenuScene,
        SampleScene,
        GameOverScene
    }

    static SceneLoader()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    public static void LoadScene(Scene scene)
    {
        if (isLoadingScene)
        {
            return;
        }

        isLoadingScene = true;
        SceneManager.LoadScene(scene.ToString());
    }   

    private static void HandleSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        isLoadingScene = false;
    }
}
