using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "Data/Item/Equippable/Weapon")]

public class WeaponItem : EquippableItem
{
    [Header("Weapon Item Setting")]
    [SerializeField] private int atkPower;

    // 프로퍼티
    public int AtkPower => atkPower;

    private void Awake()
    {
        SetEquipmentType(EquipmentType.Weapon);
    }

    /*=============== Method ===============*/

    // 아이템 장착 메서드
    public override void Equip(GameObject target)
    {
        // 아이템 효과 적용
        // Attach 등의 메서드를 통해 오브젝트를 특정 위치에 장착
        Debug.Log($"무기 아이템 장착 시도 ! | + {atkPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.EquipWeapon(this);
    }
    // 아이템 장착 해제 메서드
    public override void UnEquip(GameObject target)
    {
        // 아이템 효과 제거
        // Dettach 등의 메서드를 통해 오브젝트를 제거
        Debug.Log($"무기 아이템 장착 해제 시도 ! | - {atkPower}");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.UnEquipWeapon();
    }
}