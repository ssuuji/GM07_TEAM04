using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Data/Item/Consumable")]

public class ConsumableItem : Item
{
    [Header("Item Effects")]
    [SerializeField] private List<ItemEffect> effects;

    // 프로퍼티
    public List<ItemEffect> Effects => effects;

    private void Awake()
    {
        // 아이템 타입 소모품으로 세팅
        SetItemType(ItemType.Consumable);
    }

    /*=============== Method ===============*/

    // 소모품 사용
    public override void Use(GameObject target, int itemID)
    {
        if (target == null) return;
        // 여기에 아이템 별 제한 로직 작성
        // Ex) 체럭 포션 사용 시 체력이 가득 찼을 경우 return 등
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus == null) return;
        // 플레이어의 체력 상태를 확인 후 체력이 가득 차있다면 리턴
        if (playerStatus.CurrentHp >= playerStatus.CurrentMaxHp)
        {
            // 체력이 가득 찼다는 디버그
            Debug.Log("이미 체력이 가득 차 있어 사용할 수 없습니다.");
            return;
        }
        // 아이템이 가진 사용 효과 리스트 순회
        foreach (ItemEffect effect in effects)
        {
            // 아이템 효과 사용 메서드
            effect.ExecuteEffect(target);
        }
        // 메시지가 작성되어 있다면 출력
        if (!string.IsNullOrEmpty(MessageWhenUsed))
        {
            // 최상위 부모 클래스 Item에 있는 아이템 사용 메세지 출력
            Debug.Log(MessageWhenUsed);
        }
        // 아이템을 사용했다면 제거
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(itemID, 1);
        }
    }
}