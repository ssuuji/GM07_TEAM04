using UnityEngine;

public class GameOverSceneController : MonoBehaviour
{
    //RETRY 버튼 클릭
    public void OnClickRetryButton()
    {
        GameSceneManager.Instance.LoadScene(SceneType.Game);
    }

    //TITLE 버튼 클릭
    public void OnClickTitleButton()
    {
        GameSceneManager.Instance.ResetCheckPoint();
        GameSceneManager.Instance.LoadScene(SceneType.Title);
    }
}
