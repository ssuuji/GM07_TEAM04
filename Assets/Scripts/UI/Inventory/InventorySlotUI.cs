using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 마우스 클릭 이벤트를 위한 상속

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;               // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;    // 아이템 수량을 표시할 텍스트
    [SerializeField] private GameObject equipMark;          // 장비 아이템의 장착 여부 표시 마크

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    // 부모 UI 설정
    private InventoryUI inventoryUI;

    /*=============== Method ===============*/

    // 인벤토리 부모 UI 설정
    public void Initialize(InventoryUI parent)
    {
        inventoryUI = parent;
    }
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
            PlayerEquipment playerEquipment = FindFirstObjectByType<PlayerEquipment>();
            if (playerEquipment == null) return;

            bool isEquipped = false;

            // 무기 비교 (무기가 비어있지 않고, ID가 같다면 true)
            if (playerEquipment.WeaponItem != null && playerEquipment.WeaponItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }

            // 방어구 비교 (방어구가 비어있지 않고, ID가 같다면 true)
            if (playerEquipment.ArmorItem != null && playerEquipment.ArmorItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }

            // 최종 결과에 따라 마크 활성화/비활성화
            equipMark.SetActive(isEquipped);
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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            currentItem.ItemData.Use(player, currentItem.ItemData.ItemID);

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
    // 마우스가 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 예외 처리
        // 빈 칸 호밍 시 리턴
        if (currentItem == null) return;

        // 아이템 정보 UI 데이터 갱신
        inventoryUI.ShowItemInfo(currentItem);
    }
    // 마우스가 떠났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.CloseItemInfo();
    }
}