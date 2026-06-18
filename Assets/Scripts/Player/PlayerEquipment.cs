using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerStatus playerStatus;
    
    private WeaponItem weaponItem; //장착된 무기 저장
    private ArmorItem armorItem;

    //프로퍼티
    public bool IsWeapon => weaponItem != null; //무기 장착여부
    public bool IsArmor => armorItem != null;

    public WeaponItem WeaponItem => weaponItem; // 장착된 무기 정보확인
    public ArmorItem ArmorItem => armorItem;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void EquipWeapon(WeaponItem weapon)
    {
        if (weapon == null) return;

        weaponItem = weapon;
        playerStatus.AddAttack(weaponItem.AtkPower);

        Debug.Log($"무기 장착 완료 | 현재 공격력 {playerStatus.CurrentAttack}");
    }

    public void UnEquipWeapon()
    {
        if (!IsWeapon)
        {
            Debug.Log("장착된 무기가 없습니다.");
            return;
        }

        playerStatus.RemoveAttack(weaponItem.AtkPower);
        weaponItem = null;

        Debug.Log($"무기 해제 완료 | 현재 공격력 {playerStatus.CurrentAttack}");
    }

    public void EquipArmor(ArmorItem armor)
    {
        if (armor == null) return;

        armorItem = armor;
        playerStatus.AddDefense(armorItem.DefPower);

        Debug.Log($"방어구 장착 완료 | 현재 방어력 {playerStatus.CurrentDefense}");
    }

    public void UnEquipArmor()
    {
        if (!IsArmor)
        {
            Debug.Log("장착된 방어구가 없습니다.");
            return;
        }

        playerStatus.RemoveDefense(armorItem.DefPower);
        armorItem = null;

        Debug.Log($"방어구 해제 완료 | 현재 방어력 {playerStatus.CurrentDefense}");
    }
}