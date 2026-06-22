using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryEquipmentSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    [Header("UI Setting")]
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private Image itemIcon;

    private InventoryUI inventoryUI;
    [SerializeField] private InventoryItem currentItem;

    /*=============== Method ===============*/

    // 인벤토리 부모 UI 설정
    public void Initialize(InventoryUI parent)
    {
        inventoryUI = parent;
        ClearSlot();
    }

    public void UpdateEquipmentSlot(PlayerEquipment equipment)
    {
        if (equipment == null) return;
        if (equipmentType == EquipmentType.Weapon)
        {
            // 무기가 장착되어 있다면
            if (equipment.IsWeapon)
            {
                currentItem = new InventoryItem(equipment.WeaponItem, 1);

                itemIcon.sprite = currentItem.ItemData.ItemIcon;
                itemIcon.color = new Color(1f, 1f, 1f, 1f); // 불투명하게
            }
            else
            {
                ClearSlot();
            }
        }
        else if (equipmentType == EquipmentType.Armor)
        {
            // 방어구가 장착되어 있다면
            if (equipment.IsArmor)
            {
                currentItem = new InventoryItem(equipment.ArmorItem, 1);

                itemIcon.sprite = currentItem.ItemData.ItemIcon;
                itemIcon.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                ClearSlot();
            }
        }
    }
    public void ClearSlot()
    {
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1f, 1f, 1f, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem == null) return;

        inventoryUI.ShowItemInfo(currentItem);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.CloseItemInfo();
    }
}