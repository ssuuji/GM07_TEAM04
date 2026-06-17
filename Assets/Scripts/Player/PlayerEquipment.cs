using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("플레이어 기본 스탯")]
    [SerializeField] private PlayerBaseStatSO playerStat;

    [Header("현재 장비 상태")]
    private WeaponItem weaponItem; //장착된 무기 저장
    private ArmorItem armorItem;

    //프로퍼티
    public bool IsWeapon => weaponItem != null; //무기 장착여부
    public bool IsArmor => armorItem != null;

    public WeaponItem WeaponItem => weaponItem; // 장착된 무기 정보확인
    public ArmorItem ArmorItem => armorItem;

    //최종 공격력 계산
    public int CurrentAttack
    {
        get
        {
            int attack = playerStat.Attack;

            if (IsWeapon)
            {
                attack += weaponItem.AtkPower;
            }

            return attack;
        }
    }

    //최종 방어력 계산
    public int CurrentDefense
    {
        get
        {
            int defense = playerStat.Defense;

            if (IsArmor)
            {
                defense += armorItem.DefPower;
            }

            return defense;
        }
    }

    public void EquipWeapon(WeaponItem weapon)
    {
        if (weapon == null) return;

        weaponItem = weapon;

        Debug.Log($"무기 장착 완료 | 현재 공격력 {CurrentAttack}");
    }

    public void UnEquipWeapon()
    {
        if (!IsWeapon)
        {
            Debug.Log("장착된 무기가 없습니다.");
            return;
        }

        weaponItem = null;

        Debug.Log($"무기 해제 완료 | 현재 공격력 {CurrentAttack}");
    }

    public void EquipArmor(ArmorItem armor)
    {
        if (armor == null) return;

        armorItem = armor;

        Debug.Log($"방어구 장착 완료 | 현재 방어력 {CurrentDefense}");
    }

    public void UnEquipArmor()
    {
        if (!IsArmor)
        {
            Debug.Log("장착된 방어구가 없습니다.");
            return;
        }

        armorItem = null;

        Debug.Log($"방어구 해제 완료 | 현재 방어력 {CurrentDefense}");
    }
}