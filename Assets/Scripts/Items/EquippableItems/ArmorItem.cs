using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorItem", menuName = "Data/Item/Equippable/Armor")]

public class ArmorItem : EquippableItem
{
    [Header("Armor Item Setting")]
    [SerializeField] private int baseDefPower;

    // 프로퍼티
    public int CurrentDefPower => baseDefPower + (UpgradeLevel * 2);    // 강화 수치를 반영한 실제 방어력 증가량

    private void Awake()
    {
        SetEquipmentType(EquipmentType.Armor);
    }

    /*=============== Method ===============*/

    // 아이템 장착 메서드
    public override void Equip(GameObject target)
    {
        // 아이템 효과 적용
        Debug.Log($"방어구 아이템 장착 시도 ! | + {CurrentDefPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.EquipArmor(this);
    }
    // 아이템 장착 해제 메서드
    public override void UnEquip(GameObject target)
    {
        // 아이템 효과 제거
        Debug.Log($"방어구 아이템 장착 해제 시도 ! | - {CurrentDefPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.UnEquipArmor();
    }
}