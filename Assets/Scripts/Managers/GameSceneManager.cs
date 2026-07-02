using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public static bool IsSceneLoading { get; private set; }
    public Vector3 lastCheckPoint { get; private set; }
    public bool IsCheckPointSave { get; private set; }

    private void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IsSceneLoading = false;
    }

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
        IsSceneLoading = true;

        string sceneName = SceneNames.GetSceneName(sceneType);
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadCurrentScene()
    {
        IsSceneLoading = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
