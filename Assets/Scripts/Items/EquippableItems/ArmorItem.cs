using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorItem", menuName = "Data/Item/Equippable/Armor")]

public class ArmorItem : EquippableItem
{
    [Header("Armor Item Setting")]
    [SerializeField] private int defPower;

    // 프로퍼티
    public int DefPower => defPower;

    private void Awake()
    {
        SetEquipmentType(EquipmentType.Armor);
    }

    /*=============== Method ===============*/

    // 아이템 장착 메서드
    public override void Equip(GameObject target)
    {
        // 아이템 효과 적용
        // Attach 등의 메서드를 통해 오브젝트를 특정 위치에 장착
        Debug.Log($"방어구 아이템 장착 시도 ! | + {defPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.EquipArmor(this);
    }
    // 아이템 장착 해제 메서드
    public override void UnEquip(GameObject target)
    {
        // 아이템 효과 제거
        // Dettach 등의 메서드를 통해 오브젝트를 제거
        Debug.Log($"방어구 아이템 장착 해제 시도 ! | - {defPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.UnEquipArmor();
    }
}