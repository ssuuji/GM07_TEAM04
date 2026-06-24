using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "Data/Item/Equippable/Weapon")]

public class WeaponItem : EquippableItem
{
    [Header("Weapon Item Setting")]
    [SerializeField] private int baseAtkPower;

    // 프로퍼티
    public int CurrentAtkPower => baseAtkPower + (UpgradeLevel * 3);    // 강화 수치를 반영한 실제 공격력 증가량

    private void Awake()
    {
        SetEquipmentType(EquipmentType.Weapon);
    }

    /*=============== Method ===============*/

    // 아이템 장착 메서드
    public override void Equip(GameObject target)
    {
        // 아이템 효과 적용
        Debug.Log("무기 아이템 장착 시도 !");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.EquipWeapon(this);
    }
    // 아이템 장착 해제 메서드
    public override void UnEquip(GameObject target)
    {
        // 아이템 효과 제거
        Debug.Log("무기 아이템 장착 해제 시도 !");

        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        playerEquipment.UnEquipWeapon();
    }
}