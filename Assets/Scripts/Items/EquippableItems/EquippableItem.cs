using UnityEngine;

public abstract class EquippableItem : Item
{
    [Header("Equipment Settings")]
    [SerializeField] private EquipmentType equipType;

    // 프로퍼티
    public EquipmentType EquipType => equipType;

    private void Awake()
    {
        // 아이템 타입 장비로 세팅
        SetItemType(ItemType.Equipment);
    }

    /*=============== Method ===============*/

    // 장비 장착
    public override void Use(GameObject target, int itemID)
    {
        // Equip/UnEquip을 이용해 조건에 맞게 아이템 장착/해제를 담당할 메서드
        if (target == null) return;
        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        if (playerEquipment == null) return;

        bool isEquipped = (playerEquipment.WeaponItem != null && playerEquipment.WeaponItem.ItemID == itemID) ||
                      (playerEquipment.ArmorItem != null && playerEquipment.ArmorItem.ItemID == itemID);

        if (isEquipped)
        {
            // 이미 장착 중이면 해제
            this.UnEquip(target);
        }
        else
        {
            // 장착 중이 아니면 새로 장착
            this.Equip(target);
        }
    }

    // 장착/해제 시 효과를 작성할 추상 메서드
    public abstract void Equip(GameObject target);
    public abstract void UnEquip(GameObject target);

    // 장비 타입 세팅
    protected void SetEquipmentType(EquipmentType equipType)
    {
        this.equipType = equipType;
    }
}