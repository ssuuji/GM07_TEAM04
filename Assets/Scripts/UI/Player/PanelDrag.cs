using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Header("스탯창")]
    [SerializeField] private RectTransform statusPanel;

    private RectTransform parentRect;
    private Vector2 offset;

    private void Start()
    {
        //스탯창의 부모영역 범위를 가져와서 스탯창이 움직일 수 있는 기준 공간을 잡는다(내부좌표 변환기준)
        parentRect = statusPanel.parent as RectTransform;
    }

    //드래그 시작 할 때 마우스와 창 사이 거리를 저장 : 마우스를 누른 위치 그대로 옮기기 위해서
    public void OnBeginDrag(PointerEventData eventData)
    {
        //부모영역 기준의 로컬좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,                  //부모영역(PlayerUI)
            eventData.position,          //마우스 위치
            eventData.pressEventCamera,  //클릭할때 사용된 카메라정보
            out Vector2 mousePosition    //변환된 좌표를 저장 할 변수
            );

        //창 잡은 위치가 유지되도록 스탯창 위치와 마우스 위치사이의 거리를 저장
        offset = statusPanel.anchoredPosition - mousePosition;
    }

    //드래그 중 창 이동
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePosition
            );

        //저장했던 offset 을 더해서 처음 마우스로 잡았던 위치 그대로 창이 따라오도록 이동시킨다
        statusPanel.anchoredPosition = mousePosition + offset;
    }
}
