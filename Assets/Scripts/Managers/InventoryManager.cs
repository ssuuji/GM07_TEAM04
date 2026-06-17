using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory Setting")]
    [SerializeField] private int maxSize = 10;

    [Header("Starting Items")]
    [SerializeField] private List<InventoryItem> startingItems = new List<InventoryItem>();

    [Header("Inventory Data")]
    [SerializeField] private Dictionary<int, InventoryItem> inventoryDictionary = new Dictionary< int, InventoryItem>();

    // 프로퍼티
    public int MaxSize => maxSize;
    public List<InventoryItem> StartingItems => startingItems;
    public Dictionary<int, InventoryItem> InventoryDictionary => inventoryDictionary;

    private void Start()
    {
        // 초기 소지 아이템 세팅
        foreach (InventoryItem startingItem in startingItems)
        {
            // 예외 처리
            if (startingItem.ItemData == null || startingItem.Amount <= 0) return;
            // 아이템 저장
            AddItem(startingItem.ItemData, startingItem.Amount);
        }
    }

    /*=============== Method ===============*/
    
    // 아이템 획득 메서드
    // 아이템 획득 성공 시 true 반환
    // 아이템 획득 실패 시 false 반환
    public bool AddItem(Item newItem, int count = 1)
    {
        // 획득 시 추가 검사가 필요한 애들부터
        // 획득한 아이템이 소모품일 경우
        if (newItem.ItemType == ItemType.Consumable)
        {
            // 인벤토리 탐색
            foreach (var item in inventoryDictionary)
            {
                // 소지 중인 아이템의 ID 값이 새로 들어온 아이템의 ID값과 같은지,
                // 소지 중인 아이템의 소지 개수가 새로 들어온 아이템의 최대 소지 개수보다 작은지 확인
                if (item.Value.ItemData.ItemID == newItem.ItemID &&
                    item.Value.Amount < newItem.MaxAmount)
                {
                    // 아이템 소지 개수 증가
                    item.Value.AddAmount(count);
                    Debug.Log($"[Get New ConsumableItem] | [{newItem.ItemName}] + {item.Value.Amount}");
                    // 아이템 획득 성공
                    return true;
                }
            }
        }
        // 인벤토리의 크기가 가득 찼을 경우
        if (inventoryDictionary.Count >= maxSize)
        {
            Debug.Log("[Inventory is full]");
            // 아이템 획득 실패
            return false;
        }
        // 위 두 조건 모두 아닐 경우
        // 중복 획득이 가능한 소모품이 아닌 경우 (아이템 획득 시 무조건 새로운 슬롯을 필요로 하는 경우)
        // 인벤토리가 가득 차지 않은 경우 (빈 아이템 슬롯이 존재하는 경우)
        // 인벤토리에 아이템 저장
        InventoryItem newItemInstance = new InventoryItem(newItem, count);
        inventoryDictionary.Add(newItemInstance.ItemData.ItemID, newItemInstance);
        Debug.Log($"[Get New EquippableItem] | [{newItem.ItemName}] + {count}");
        // 아이템 획득 성공
        return true;
    }
    // 아이템 제거 메서드
    public void RemoveItem(int itemID, int count = 1)
    {
        // 예외 처리
        // 인벤토리에 입력된 ID를 가진 아이템이 없다면 리턴
        if (!inventoryDictionary.ContainsKey(itemID)) return;
        // 입력된 ID를가진 아이템 저장
        InventoryItem targetItem = inventoryDictionary[itemID];
        // 아이템 수량 감소
        targetItem.SubAmount(count);
        // 남은 아이템 수량이 없다면 제거
        if (targetItem.Amount <= 0)
        {
            inventoryDictionary.Remove(itemID);
            Debug.Log($"{targetItem.ItemData.ItemName} is removed");
        }
        else
        {
            Debug.Log($"{targetItem.ItemData.ItemName} x{targetItem.Amount}");
        }
    }
    // 인벤토리 확장 메서드
    public void ExpandInventory(int amount)
    {
        // 최대 크기 값 증가
        maxSize += amount;
        Debug.Log($"Inventory Size Up! + {amount} | Max : {maxSize}");
    }
}