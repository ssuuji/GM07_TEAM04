using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 마우스 클릭 이벤트를 위한 상속

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;               // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;    // 아이템 수량을 표시할 텍스트
    [SerializeField] private GameObject equipMark;          // 장비 아이템의 장착 여부 표시 마크

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    /*=============== Method ===============*/
    
    public void UpdateSlot(InventoryItem item)
    {
        currentItem = item;
        // 아이콘 이미지 적용 및 투명도 조절
        iconImage.sprite = item.ItemData.ItemIcon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // 아이템 수량 표시
        // 중복 아이템이 2개 이상부터 표시
        if (item.Amount > 1)
        {
            amountText.text = item.Amount.ToString();
            amountText.gameObject.SetActive(true);
        }
        else
        {
            amountText.gameObject.SetActive(false);
        }
        // 장비 아이템이라면 장착 여부 표시
        // 예외 처리
        if (equipMark == null) return;
        if (item.ItemData.ItemType == ItemType.Equipment)
        {
            // 장착 여부 정보를 플레이어에게 받아와 조건 부 실행

        }
        else
        {
            // 소모품은 장착 마크 비활성화
            equipMark.SetActive(false);
        }
    }
    // 슬롯 초기화 메서드
    public void ClearSlot()
    {
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
        // 예외 처리
        // 빈 칸 클릭 시 리턴
        if (currentItem == null) return;

        // 마우스 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LeftClick");

            // 아이템 사용 메서드 호출
            //currentItem.ItemData.Use(플레이어 객체, currentItem.ItemData.ItemID);
            

            // UI 갱신
            InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
            if (inventoryUI == null) return;
            inventoryUI.RefreshUI();
        }
        // 마우스 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RightClick");
        }
    }
}