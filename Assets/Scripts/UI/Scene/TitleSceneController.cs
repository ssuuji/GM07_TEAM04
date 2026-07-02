using DG.Tweening;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    [Header("화면 페이드")]
    [SerializeField] private CanvasGroup fadeCanvasGroup; // 화면 페이드 처리용
    [SerializeField] private float fadeInTime = 1.2f;     // 화면 페이드 시간

    [Header("타이틀 등장 연출")]
    [SerializeField] private RectTransform titleText;      // 타이틀 위치/크기 제어용
    [SerializeField] private CanvasGroup titleCanvasGroup; // 타이틀 페이드 처리용
    private Vector2 titleOriginPos;

    [Header("타이틀 배경음")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip titleBGM;

    private void Awake()
    {
        titleOriginPos = titleText.anchoredPosition; // 원래 타이틀 위치 저장
    }

    private void Start()
    {
        PlayScreenFadeIn();
        PlayTitleIntro();
        PlayBGM();
    }

    private void PlayBGM()
    {
        audioSource.clip = titleBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    // 검은 화면에서 타이틀 화면이 서서히 보이도록 처리
    private void PlayScreenFadeIn()
    {
        fadeCanvasGroup.alpha = 1f;

        fadeCanvasGroup.DOFade(0f, fadeInTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            fadeCanvasGroup.blocksRaycasts = false;
        });
    }

    // 타이틀 등장 연출
    private void PlayTitleIntro()
    {
        titleCanvasGroup.alpha = 0f;                                        // 시작 시 안 보이게 설정
        titleText.anchoredPosition = titleOriginPos + new Vector2(0f, 80f); // 원래 위치보다 위에서 시작
        titleText.localScale = Vector3.one * 0.75f;                         // 시작 시 살짝 작게 설정

        Sequence titleSequence = DOTween.Sequence();
        titleSequence.AppendInterval(0.4f);                                                      // 화면이 조금 보이기 시작한 뒤 타이틀 등장
        titleSequence.Append(titleText.DOAnchorPos(titleOriginPos, 1.2f).SetEase(Ease.OutBack)); // 아래로 내려오면서 등장
        titleSequence.Join(titleText.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack));          // 이동과 동시에 원래 크기로 확대
        titleSequence.Join(titleCanvasGroup.DOFade(1f, 0.8f));                                   // 이동과 동시에 페이드 인
    }

    //START 버튼 클릭
    public void OnClickStartButton()
    {
        GameManager.Instance.ResetStats();
        GameManager.Instance.StartTimer();

        GameSceneManager.Instance.LoadScene(SceneType.Game);
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
