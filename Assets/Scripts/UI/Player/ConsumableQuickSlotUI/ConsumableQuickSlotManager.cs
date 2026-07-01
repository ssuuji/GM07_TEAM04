using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableQuickSlotManager : Singleton<ConsumableQuickSlotManager>
{
    // 1~3번 퀵슬롯에 저장된 소모품 데이터
    public ConsumableItem[] quickSlots = new ConsumableItem[3];
    // 슬롯별 쿨타임 상태 확인
    private List<int> coolDownItemIDs = new List<int>();

    // 이벤트
    // UI 갱신을 위한 이벤트
    public event Action OnQuickSlotUpdated;
    // 쿨타임 시작을 UI 슬롯들에게 알리는 이벤트
    public event Action<int, float> OnItemCooldownStarted;

    private void Update()
    {
        // 각 숫자 키 입력 시 대응하는 아이템 사용
        if (InputManager.IsQuickSlot1)
        {
            UseQuickSlotItem(0);
        }
        if (InputManager.IsQuickSlot2)
        {
            UseQuickSlotItem(1);
        }
        if (InputManager.IsQuickSlot3)
        {
            UseQuickSlotItem(2);
        }
    }

    /*=============== Method ===============*/
    
    // 슬롯 UI 갱신 메서드
    public void UseQuickSlotItem(int slotIndex)
    {
        // 입력받은 슬롯의 아이템 저장
        ConsumableItem currentItem = quickSlots[slotIndex];
        if (currentItem == null) return; 
        if (IsItemOnCooldown(currentItem.ItemID)) return;
        // 아이템의 남은 개수에 대한 정보를 받아와 소지 개수가 없다면 리턴
        if (InventoryManager.Instance.GetConsumableItemAmount(currentItem.ItemID) <= 0)
        {
            Debug.Log($"[{currentItem.ItemName}] 아이템 소지 개수가 부족합니다!");
            MessageUI message = FindFirstObjectByType<MessageUI>();
            message.ShowMessage($"[{currentItem.ItemName}] 아이템 소지 개수가 부족합니다!");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        // 아이템 사용
        currentItem.Use(player, currentItem.ItemID);
        // UI 업데이트 하라는 이벤트
        OnQuickSlotUpdated?.Invoke();
    }
    // 슬롯 아이템 설정 메서드
    public void SetQuickSlotItem(int slotIndex, ConsumableItem item)
    {
        // 슬롯에 아이템 저장
        quickSlots[slotIndex] = item;
        // UI 업데이트 하라는 이벤트
        OnQuickSlotUpdated?.Invoke();
    }
    // 슬롯 비우는 메서드
    public void RemoveSlotItem(int slotIndex)
    {
        // 슬롯 데이터 비우기
        quickSlots[slotIndex] = null;
        // UI 업데이트 하라는 이벤트
        OnQuickSlotUpdated?.Invoke();
    }
    // 쿨타임 상태 반환 메서드
    public bool IsItemOnCooldown(int itemID)
    {
        return coolDownItemIDs.Contains(itemID);
    }
    // 쿨타임 시작 메서드
    public void StartItemCooldown(int itemID, float duration)
    {
        if (coolDownItemIDs.Contains(itemID) || duration <= 0f) return;
        StartCoroutine(ItemCooldownCo(itemID, duration));
    }
    // 쿨타임 진행 코루틴
    private IEnumerator ItemCooldownCo(int itemID, float duration)
    {
        coolDownItemIDs.Add(itemID);

        // 아이템이 사용 됐으니 쿨타임을 실행하라는 이벤트
        OnItemCooldownStarted?.Invoke(itemID, duration);

        yield return new WaitForSeconds(duration);

        coolDownItemIDs.Remove(itemID);
    }
}