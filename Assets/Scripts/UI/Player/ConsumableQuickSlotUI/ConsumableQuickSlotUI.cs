using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumableQuickSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("Slot Number")]
    [SerializeField] private int slotIndex;                 // 슬롯 번호

    [Header("UI Binding")]
    [SerializeField] private Image itemIcon;                // 슬롯에 등록된 아이템 아이콘
    [SerializeField] private TextMeshProUGUI amountText;    // 슬롯에 등록된 아이템 소지 개수
    [SerializeField] private SlotCoolDown slotCoolDown;

    private void Start()
    {
        // 이벤트에 UI 갱신 메서드 등록
        ConsumableQuickSlotManager.Instance.OnQuickSlotUpdated += RefreshUI;
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        // 
        ConsumableQuickSlotManager.Instance.OnItemCooldownStarted += ItemCooldown;
        // 시작 시 UI 갱신
        RefreshUI();
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 매니저가 존재한다면
        // 이벤트 구독 해제
        if (ConsumableQuickSlotManager.Instance != null)
        {
            ConsumableQuickSlotManager.Instance.OnQuickSlotUpdated -= RefreshUI;
            ConsumableQuickSlotManager.Instance.OnItemCooldownStarted -= ItemCooldown;
        }
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    /*=============== Method ===============*/

    // UI 갱신 메서드
    public void RefreshUI()
    {
        // 인덱스 번호에 맞는 아이템 저장
        ConsumableItem item = ConsumableQuickSlotManager.Instance.quickSlots[slotIndex];
        if (item != null)
        {
            // 인벤토리 매니저에서 아이템 소지 개수 받아오기
            int amount = InventoryManager.Instance.GetConsumableItemAmount(item.ItemID);
            // 개수가 0보다 크다면 (소지하고 있다면)
            if (amount > 0)
            {
                // UI 갱신
                itemIcon.sprite = item.ItemIcon;
                itemIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                amountText.text = amount.ToString();
                return;
            }
        }
        // 슬롯에 아이템이 존재하지 않는다면 비우기
        itemIcon.sprite = null;
        itemIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        amountText.text = "";
    }
    // 인벤토리에서 드래그 해온 아이템 UI가 드랍될 때
    public void OnDrop(PointerEventData eventData)
    {
        // 아이템 슬롯 유형 및 아이템 데이터 존재 확인
        InventorySlotUI slot = eventData.pointerDrag.GetComponent<InventorySlotUI>();
        if (slot == null || slot.CurrentItem == null) return;
        // 슬롯 데이터에 저장되어 있는 아이템을 소모품으로 캐스팅
        ConsumableItem item = slot.CurrentItem.ItemData as ConsumableItem;
        if (item == null) return;
        // 인덱스 번호가 맞는 퀵슬롯에 아이템 등록
        ConsumableQuickSlotManager.Instance.SetQuickSlotItem(slotIndex, item);
    }
    // 퀵슬롯 UI를 클릭했을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ConsumableQuickSlotManager.Instance.UseQuickSlotItem(slotIndex);
        }
        // 우클릭
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 클릭이 됐는지 확인 메서드
            Debug.Log("RightClick");
            // 퀵슬롯이 비어있다면 리턴
            if (ConsumableQuickSlotManager.Instance.quickSlots[slotIndex] == null) return;
            // 퀵슬롯에 아이템이 저장되어 있다면 제거
            ConsumableQuickSlotManager.Instance.RemoveSlotItem(slotIndex);
        }
    }
    private void ItemCooldown(int itemID, float duration)
    {
        // 
        ConsumableItem myItem = ConsumableQuickSlotManager.Instance.quickSlots[slotIndex];

        // 
        if (myItem != null && myItem.ItemID == itemID)
        {
            if (slotCoolDown != null)
            {
                slotCoolDown.StartCooldown(duration);
            }
        }
    }
}