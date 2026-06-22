using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDoTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    [Header("마우스 오버 크기")]
    [SerializeField] private float hoveScale = 1.09f; //마우스 올렸을 때 크기
    [SerializeField] private float duration = 0.15f;  //확대/축소 시간

    private Vector3 originScale;

    private void Start()
    {
        //기존 버튼 크기 저장
        originScale = transform.localScale;
    }

    //마우스 버튼 위에 올렸을 때
    public void OnPointerEnter(PointerEventData evnetData)
    {
        transform.DOKill();
        transform.DOScale(originScale * hoveScale, 0.15f).SetEase(Ease.OutQuad);

    }

    //마우스가 버튼 밖으로 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale, 0.15f).SetEase(Ease.OutQuad);
    }
}
