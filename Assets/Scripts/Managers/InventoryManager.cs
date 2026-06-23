using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory Setting")]
    [SerializeField] private int maxSize = 10;  // 인벤토리 크기

    [Header("Starting Items")]
    [SerializeField] private List<InventoryItem> startingItems = new List<InventoryItem>();     // 초기 지급 될 아이템 목록

    [Header("Inventory Data")]
    // 인벤토리 내부 데이터를 관리할 자료구조
    // Key값으로 ItemID값을 사용하고 그에 맞는 아이템 데이터를 매칭해 저장
    // 장비 아이템은 딱 한 개만 존재하므로(모든 아이템은 MaxAmount값 즉, 최대 소지 개수를 초과할 일이 없으므로) UID를 통한 string값 관리가 아닌 ItemID를 통한 int값으로 관리
    [SerializeField] private Dictionary<int, InventoryItem> inventoryDictionary = new Dictionary<int, InventoryItem>();

    // 프로퍼티
    public int MaxSize => maxSize;
    public List<InventoryItem> StartingItems => startingItems;
    public Dictionary<int, InventoryItem> InventoryDictionary => inventoryDictionary;

    private void Start()
    {
        // 초기 소지 아이템 지급
        // 초기 지급 아이템 목록 확인
        foreach (InventoryItem startingItem in startingItems)
        {
            // 아이템 데이터가 존재하고, 수량이 1개 이상이라면
            if (startingItem.ItemData != null && startingItem.Amount > 0)
            {
                // 인벤토리에 아이템 추가
                AddItem(startingItem.ItemData, startingItem.Amount);
            }
        }
        // 아이템을 저장(추가)했으니 인벤토리 UI 갱신
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }
    }

    /*=============== Method ===============*/
    
    // 아이템 획득 메서드
    // 아이템 획득 성공 시 true 반환
    // 아이템 획득 실패 시 false 반환
    public bool AddItem(Item newItem, int count = 1)
    {
        // UI 갱신용
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI == null) return false;
        // 획득 시 추가 검사가 필요한 애들부터
        // 획득한 아이템이 골드일 경우
        if (newItem.ItemType == ItemType.Gold)
        {
            // 획윽한 아이템을 골드 아이템으로 형변환
            GoldItem goldItem = newItem as GoldItem;
            // 골드 증가
            GoldManager.Instance.AddGold(goldItem.GoldAmount * count);
            // 획득 완료
            return true;
        }
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
                    // 아이템 획득 확인용 디버그
                    Debug.Log($"[Get New ConsumableItem] | [{newItem.ItemName}] + {item.Value.Amount}");
                    // 인벤토리 UI 갱신
                    inventoryUI.RefreshUI();
                    // 아이템 획득 성공
                    return true;
                }
            }
        }
        // 인벤토리의 크기가 가득 찼을 경우
        if (inventoryDictionary.Count >= maxSize)
        {
            // 아이템 획득 실패 이유 디버그
            Debug.Log("[Inventory is full]");
            // 아이템 획득 실패
            return false;
        }
        // 위 두 조건 모두 아닐 경우
        // + 중복 획득이 가능한 소모품이 아닌 경우 (아이템 획득 시 무조건 새로운 슬롯을 필요로 하는 경우)
        // + 인벤토리가 가득 차지 않은 경우 (빈 아이템 슬롯이 존재하는 경우)
        // 인벤토리에 아이템 저장
        // 아이템 생성 및 저장
        InventoryItem newItemInstance = new InventoryItem(newItem, count);
        // 인벤토리 딕셔너리에 아이템 ID값을 Key값으로 지정해서 추가
        inventoryDictionary.Add(newItemInstance.ItemData.ItemID, newItemInstance);
        // 아이템 획득 완료 디버그
        Debug.Log($"[Get New EquippableItem] | [{newItem.ItemName}] + {count}");
        // 인벤토리 UI 갱신
        inventoryUI.RefreshUI();
        // 아이템 획득 성공
        return true;
    }
    // 아이템 제거 메서드
    public void RemoveItem(int itemID, int count = 1)
    {
        // 인벤토리에 매개변수로 입력된 ID를 가진 아이템이 없다면 리턴
        if (!inventoryDictionary.ContainsKey(itemID)) return;
        // 입력된 ID를가진 아이템 생성 및 저장
        InventoryItem targetItem = inventoryDictionary[itemID];
        // 아이템 수량 감소
        targetItem.SubAmount(count);
        // 남은 아이템 수량이 없다면 제거
        if (targetItem.Amount <= 0)
        {
            // 딕셔너리에서 Key(ItemID)값을 통해 제거
            inventoryDictionary.Remove(itemID);
            // 제거 완료 디버그
            Debug.Log($"{targetItem.ItemData.ItemName} is removed");
        }
        // 아이템 수량이 남았다면
        else
        {
            // 아이템 수량 감소 디버그
            Debug.Log($"{targetItem.ItemData.ItemName} x{targetItem.Amount}");
        }
        // 아이템 제거가 완료 됐다면
        // 인벤토리 UI 갱신
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }
    }
    // 인벤토리 확장 메서드
    public void ExpandInventory(int amount)
    {
        // 최대 크기 값 증가
        maxSize += amount;
        // 최대 크기 증가 확인 디버그
        Debug.Log($"Inventory Size Up! + {amount} | Max : {maxSize}");
    }
}