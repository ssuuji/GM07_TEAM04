using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EquippableItem : Item
{
    [Header("Equipment Settings")]
    [SerializeField] private EquipmentType equipType;   // 아이템 종류
    // 아무래도 강화 정보는 여기서 처리해야겠지?
    [Header("Upgrade Data")]
    [SerializeField] private int upgradeLevel = 0;      // 아이템 강화 수치
    [SerializeField] private int maxUpgradeLevel = 5;   // 아이템 최대 강화 수치

    // 프로퍼티
    public EquipmentType EquipType => equipType;
    public int UpgradeLevel => upgradeLevel;
    public int MaxUpgradeLevel => maxUpgradeLevel;

    private void Awake()
    {
        // 아이템 타입 장비로 세팅
        SetItemType(ItemType.Equipment);
    }

    /*=============== Method ===============*/

    // 장비 장착
    // Equip/UnEquip을 이용해 조건에 맞게 아이템 장착/해제를 담당할 메서드
    public override void Use(GameObject target, int itemID)
    {
        if (target == null) return;
        PlayerEquipment playerEquipment = target.GetComponent<PlayerEquipment>();
        if (playerEquipment == null) return;
        // 현재 아이템의 장착 여부 확인
        // 플레이어가 장비를 장착 중인지 + 장착된 아이템의 ID값과 이 아이템의 ID값이 같은지 확인
        bool isEquipped = (playerEquipment.WeaponItem != null && playerEquipment.WeaponItem.ItemID == itemID) ||
                          (playerEquipment.ArmorItem != null && playerEquipment.ArmorItem.ItemID == itemID);
        // 장착 중인 아이템이라면
        if (isEquipped)
        {
            // 장착 해제
            this.UnEquip(target);
        }
        // 장착 중인 아이템이 아니라면
        else
        {
            // 장착 중인 아이템 확인
            if ((playerEquipment.IsWeapon) || (playerEquipment.IsArmor))
            {
                // 장착 해제
                this.UnEquip(target);
            }
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
    // 장비 강화 메서드
    public virtual bool UpgradeItem()
    {
        // 실패 조건부터 처리
        // 최대 강화 수치일 경우
        if (upgradeLevel >= maxUpgradeLevel)
        {
            Debug.Log("최대 강화 수치입니다.");
            return false;
        }
        if (GoldManager.Instance == null) return false;
        // 장비 강화 재화 설정
        int upgradePrice = 100 + (upgradeLevel * 100);
        // 골드 지급
        bool isUpgrade = GoldManager.Instance.SpendGold(upgradePrice);
        // 소지 골드가 부족할 경우
        if (!isUpgrade) return false;
        // 강화 수치 증가
        upgradeLevel++;
        Debug.Log($"강화 성공! | {ItemName} +{upgradeLevel}");
        // 강화 성공
        return true;
    }
}