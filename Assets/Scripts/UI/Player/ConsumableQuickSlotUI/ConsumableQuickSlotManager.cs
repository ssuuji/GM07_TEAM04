using System;
using UnityEngine;

public class ConsumableQuickSlotManager : Singleton<ConsumableQuickSlotManager>
{
    // 1~3번 퀵슬롯에 저장된 소모품 데이터
    public ConsumableItem[] quickSlots = new ConsumableItem[3];
    
    // UI 갱신을 위한 이벤트
    public event Action OnQuickSlotUpdated;

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
        // 아이템의 남은 개수에 대한 정보를 받아와 소지 개수가 없다면 리턴
        if (InventoryManager.Instance.GetConsumableItemAmount(currentItem.ItemID) <= 0)
        {
            Debug.Log($"[{currentItem.ItemName}] 아이템 소지 개수가 부족합니다!");
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
}