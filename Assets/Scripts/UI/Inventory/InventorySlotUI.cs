using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 마우스 클릭 이벤트를 위한 상속

public class InventorySlotUI : MonoBehaviour, 
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,    // 마우스 클릭, 커서가 겹칠 때, 커서가 나갈 때
    IBeginDragHandler,IDragHandler, IEndDragHandler                     // 드래그 시작, 드래그, 드래그 끝
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;               // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;    // 아이템 수량을 표시할 텍스트
    [SerializeField] private GameObject equipMark;          // 장비 아이템의 장착 여부 표시 마크

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    // UI가 생성될 위치 설정
    [Header("Panel Settings")]
    private RectTransform rectTransform;
    private Vector2 originPosition; 

    // 프로퍼티
    public InventoryItem CurrentItem => currentItem;

    // 슬롯을 다른 곳에도 쓰기 위해 부모 UI를 직접 설정하는 방식에서 이벤트 형식으로 변경
    private Action<InventoryItem> onClickAction;    // 슬롯이 클릭될 때
    private Action<InventoryItem> onEnterAction;    // 슬롯 위로 커서가 올라올 때
    private Action onExitAction;                    // 슬롯에서 커서가 나갈 때

    private void Awake()
    {
        // 컴포넌트 받아오기
        rectTransform = GetComponent<RectTransform>();
    }

    /*=============== Method ===============*/

    // 이벤트를 활용(전달)할 메서드
    public void Initialize(Action<InventoryItem> onClick, Action<InventoryItem> onEnter, Action onExit)
    {
        onClickAction = onClick;
        onEnterAction = onEnter;
        onExitAction = onExit;
    }
    // 슬롯 내부 데이터 갱신 메서드
    public void UpdateSlot(InventoryItem item)
    {
        // 새로 들어온 아이템 저장
        currentItem = item;
        // 아이콘 이미지 적용 및 투명도 조절
        iconImage.sprite = item.ItemData.ItemIcon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // 아이템 수량 표시
        // 중복 아이템이 2개 이상부터 표시
        if (item.Amount > 1)
        {
            // 소지 수량 string으로 변환해 text 내용 변환
            amountText.text = item.Amount.ToString();
            // 수량 오브젝트 켜기
            amountText.gameObject.SetActive(true);
        }
        // 한 개만 가지고 있다면
        else
        {
            // 수량 오브젝트 끄기
            amountText.gameObject.SetActive(false);
        }
        // 장비 아이템이라면 장착 여부 표시
        if (equipMark == null) return;
        if (item.ItemData.ItemType == ItemType.Equipment)
        {
            // 장착 여부 정보를 플레이어 장착 관리 스크립트에서 받아와 조건 부 실행
            PlayerEquipment playerEquipment = FindFirstObjectByType<PlayerEquipment>();
            if (playerEquipment == null) return;
            // 장착 여부 판단 변수
            bool isEquipped = false;
            // 무기 비교 (무기가 장착되어 있고, 장착되어 있는 아이템의 ID값이 검사하는 슬롯의 아이템 ID값과 같다면 true)
            if (playerEquipment.WeaponItem != null && playerEquipment.WeaponItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }
            // 방어구 비교 (방어구가 장착되어 있고,장착되어  있는 아이템의 ID값이 검사하는 슬롯의 아이템 ID값과 같다면 true)
            if (playerEquipment.ArmorItem != null && playerEquipment.ArmorItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }
            // 최종 결과에 따라 마크 활성화/비활성화
            equipMark.SetActive(isEquipped);
        }
        else
        {
            // 장비가 아닌 아이템들은 장착 마크 비활성화
            equipMark.SetActive(false);
        }
    }
    // 슬롯 초기화 메서드
    public void ClearSlot()
    {
        // 다 비우고 끄고 투명하게
        currentItem = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        amountText.gameObject.SetActive(false);
        if (equipMark == null) return;
        equipMark.SetActive(false);
    }
    // 유니티 제공 마우스 클릭 이벤트 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        // 빈 칸 클릭 시 리턴
        if (currentItem == null) return;
        // 마우스 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 클릭이 됐는지 확인 디버그
            Debug.Log("LeftClick");
            // 클릭이 되었다는 이벤트 알림
            onClickAction?.Invoke(currentItem);
        }
        // 마우스 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 클릭이 됐는지 확인 메서드
            Debug.Log("RightClick");
        }
    }
    // 마우스가 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 빈 칸일 시 리턴
        if (currentItem == null) return;
        // 아이템 정보 UI 활성화
        // 커서가 슬롯 위로 올라왔다는 이벤트 알림
        onEnterAction?.Invoke(currentItem);
    }
    // 마우스가 떠났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        // 아이템 정보 UI 비활성화
        // 커서가 슬롯 밖으로 나갔다는 이벤트 알림
        onExitAction?.Invoke();
    }
    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템이 존재하지 않다면 리턴
        //if (currentItem == null) return;
        // 드래그 완료 후 UI가 돌아올 위치 저장
        originPosition = rectTransform.anchoredPosition;
    }
    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        // UI 이동
        FollowCursor();
    }
    // 드레그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        // 원래 위치로 이동
        SetOriginPosition();
    }
    // 커서를 따라 이동하는 메서드
    private void FollowCursor()
    {
        // InputSystem을 통해 구한 마우스 위치값
        Vector2 mousePos = Mouse.current.position.ReadValue();
        // 위치값 전달
        rectTransform.position = mousePos;
    }
    // 저장되어있는 기본 위치로 이동하는 메서드
    private void SetOriginPosition()
    {
        // 초기 위치값으로 변경
        rectTransform.anchoredPosition = originPosition;
    }
}