using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public Vector3 lastCheckPoint { get; private set; }
    public bool IsCheckPointSave { get; private set; }

    public void SetCheckPoint(Vector3 position)
    {
        lastCheckPoint = position;
        IsCheckPointSave = true;
    }

    public void ResetCheckPoint()
    {
        lastCheckPoint = Vector3.zero;
        IsCheckPointSave = false;
    }
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
