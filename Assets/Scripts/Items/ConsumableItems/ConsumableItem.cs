using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "Data/Item/Consumable")]

public class ConsumableItem : Item
{
    [Header("Item Effects")]
    [SerializeField] private List<ItemEffect> effects;  // 아이템 사용 시 발생할 효과들 목록
    [Header("Use Setting")]
    [SerializeField] private float coolDown;            // 아이템 사용 쿨타임
    [Header("Use Feedback")]
    [SerializeField] private AudioClip useSFX;      // 아이템 사용 시 출력할 사운드
    [SerializeField] private GameObject useVFX;     // 아이템 사용 시 출력할 이펙트

    // 프로퍼티
    public List<ItemEffect> Effects => effects;
    public float CoolDown => coolDown;

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
        if (ConsumableQuickSlotManager.Instance != null &&
        ConsumableQuickSlotManager.Instance.IsItemOnCooldown(itemID))
        {
            // 아이템 쿨타임 확인 후 쿨타임 상태라면 리턴
            Debug.Log($"[{ItemName}]은 아직 재사용 대기 중입니다.");
            return;
        }
        // 아이템이 가진 사용 효과 리스트 순회
        foreach (ItemEffect effect in effects)
        {
            if (!effect.ExecuteEffect(target)) return;
        }
        // 아이템 사용 피드백 재생
        PlayFeedback(target); 
        if (ConsumableQuickSlotManager.Instance != null)
        {
            ConsumableQuickSlotManager.Instance.StartItemCooldown(itemID, CoolDown);
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
    // 아이템 사용 피드백 효과 재생 메서드
    private void PlayFeedback(GameObject player)
    {
        if (useSFX != null)
        {
            AudioSource.PlayClipAtPoint(useSFX, player.transform.position);
        }

        if (useVFX != null)
        {
            Instantiate(useVFX, player.transform.position, Quaternion.identity);
        }
    }
}