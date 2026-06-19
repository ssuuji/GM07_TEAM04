using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private PlayerStatus playerStatus;
    
    private WeaponItem weaponItem; //현재 장착된 무기 
    private ArmorItem armorItem;   //현재 장착된 방어구

    //장비 장착여부 프로퍼티
    public bool IsWeapon => weaponItem != null; 
    public bool IsArmor => armorItem != null;

    //현재 장착된 장비 정보확인 프로퍼티
    public WeaponItem WeaponItem => weaponItem; 
    public ArmorItem ArmorItem => armorItem;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    //무기장착
    public void EquipWeapon(WeaponItem weapon)
    {
        if (weapon == null) return; //장착할 무기가 없으면 return

        //현재 무기 저장
        weaponItem = weapon;
        //무기 공격력 만큼 플레이어 공격력 증가
        playerStatus.AddAttack(weaponItem.AtkPower);

        Debug.Log($"무기 장착 완료 | 현재 공격력 {playerStatus.CurrentAttack}");
    }

    //무기해제
    public void UnEquipWeapon()
    {
        if (!IsWeapon) //장착된 무기가 없으면 return
        {
            Debug.Log("장착된 무기가 없습니다.");
            return;
        }

        //장착중인 무기 공격력만큼 플레이어 공격력 감소
        playerStatus.RemoveAttack(weaponItem.AtkPower);
        //현재 무기정보 초기화
        weaponItem = null;

        Debug.Log($"무기 해제 완료 | 현재 공격력 {playerStatus.CurrentAttack}");
    }

    //방어구 장착
    public void EquipArmor(ArmorItem armor)
    {
        if (armor == null) return; //장착할 방어구가 없으면 종료

        //현재 방어구 저장
        armorItem = armor;
        //방어구 방어력 만큼 플레이어 방어력 증가
        playerStatus.AddDefense(armorItem.DefPower);

        Debug.Log($"방어구 장착 완료 | 현재 방어력 {playerStatus.CurrentDefense}");
    }

    //방어구 해제
    public void UnEquipArmor()
    {
        if (!IsArmor) // 장착된 방어구가 없다면 return
        {
            Debug.Log("장착된 방어구가 없습니다.");
            return;
        }

        //장착중인 방어구 방어력 만큼 플레이어 방어력 감소
        playerStatus.RemoveDefense(armorItem.DefPower);
        //현재 방어구정보 초기화
        armorItem = null;

        Debug.Log($"방어구 해제 완료 | 현재 방어력 {playerStatus.CurrentDefense}");
    }
}