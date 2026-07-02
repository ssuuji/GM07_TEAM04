using TMPro;
using UnityEngine;

public class GameClearSceneController : MonoBehaviour
{
    [Header("Clear Result")]
    [SerializeField] private TextMeshProUGUI playerTime;
    [SerializeField] private TextMeshProUGUI monsterCount;

    private void Start()
    {
        ShowClearResult();
    }

    //통계 표시
    private void ShowClearResult()
    {
        playerTime.text = GameManager.Instance.GetPlayTimeText();
        monsterCount.text = GameManager.Instance.KillCount.ToString();
    }

    //TITLE 버튼 클릭
    public void OnClickTitleButton()
    {
        GameSceneManager.Instance.ResetCheckPoint();
        GameSceneManager.Instance.LoadScene(SceneType.Title);
    }

    //EXIT 버튼 클릭 시 게임 종료
    public void OnClickExitButton()
    {
        // 게임종료
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit();
        #endif
        
    }
}
