using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public void LoadScene(SceneType sceneType)
    {
        string sceneName = SceneNames.GetSceneName(sceneType);
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
