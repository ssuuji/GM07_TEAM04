using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmithSlotUI : MonoBehaviour
{
    [Header("UI Binding")]
    [SerializeField] private Image iconImage;   // 선택된 아이템 아이콘
    [SerializeField] private TextMeshProUGUI itemNameText;  // 선택된 아이템 이름

    private InventoryItem targetItem;   // 선택된 아이템

    // 프로퍼티
    public InventoryItem TargetItem => targetItem;


    private void Start()
    {
        // 슬롯 초기화
        ClearSlot();
    }

    /*=============== Method ===============*/

    // 강화할 아이템 선택
    public void SelectedItem(InventoryItem item)
    {
        if (item == null) return;
        // 선택된 아이템 설정
        targetItem = item;
        iconImage.sprite = item.ItemData.ItemIcon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // 장비 아이템으로 캐스팅
        if (item.ItemData is EquippableItem equip)
        {
            // 아이템 이름 + 강화 수치 표시
            itemNameText.text = $"{equip.ItemName} (+{equip.UpgradeLevel})";
        }
        // 강화 UI 갱신
        SmithUI.Instance.RefreshUI();
    }
    // 슬롯 초기화
    public void ClearSlot()
    {
        targetItem = null;
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        if (itemNameText != null)
        {
            itemNameText.text = "Select Upgrade Item";
        }
    }
}