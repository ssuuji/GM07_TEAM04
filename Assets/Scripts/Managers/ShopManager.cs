using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    // 상점 가방 크기 설정
    [Header("Shop Setting")]
    [SerializeField] private int maxSize = 10;
    // 상점에서 판매할 아이템 설정
    [Header("Shop Items")]
    [SerializeField] private List<InventoryItem> shopItems;
    // 상점 가방 데이터 관리
    [Header("Shop Data")]
    [SerializeField] private Dictionary<int, InventoryItem> shopDictionary = new Dictionary<int, InventoryItem>();

    // 프로퍼티
    public int MaxSize => maxSize;
    public List<InventoryItem> ShopItem => shopItems;
    public Dictionary<int, InventoryItem> ShopDictionary => shopDictionary;

    protected override void Awake()
    {

    }

    private void Start()
    {
        // 상점 판매 아이템 세팅
        foreach (InventoryItem shopItem in  shopItems)
        {
            if (shopItem.ItemData != null && shopItem.Amount > 0)
            {
                AddItem(shopItem.ItemData, shopItem.Amount);
            }
        }
        // UI 갱신
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI != null)
        {
            shopUI.RefreshUI();
        }
    }

    /*=============== Method ===============*/

    // 아이템 저장(추가) 메서드
    public bool AddItem(Item newItem, int count = 1)
    {
        // UI 갱신용
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI == null) return false;
        // 저장 시 추가 검사가 필요한 애들부터
        // 근데 더 추가될 일이 없을 테니 이런 검사가 필요 없을 수도?
        // 획득한 아이템이 소모품일 경우
        if (newItem.ItemType == ItemType.Consumable)
        {
            // 인벤토리 탐색
            foreach (var item in shopDictionary)
            {
                // 소지 중인 아이템의 ID 값이 새로 들어온 아이템의 ID값과 같은지,
                // 소지 중인 아이템의 소지 개수가 새로 들어온 아이템의 최대 소지 개수보다 작은지 확인
                if (item.Value.ItemData.ItemID == newItem.ItemID &&
                    item.Value.Amount < newItem.MaxAmount)
                {
                    // 아이템 소지 개수 증가
                    item.Value.AddAmount(count);
                    // UI 갱신
                    shopUI.RefreshUI();
                    // 아이템 획득 성공
                    return true;
                }
            }
        }
        // 상점의 크기가 가득 찼을 경우
        if (shopDictionary.Count >= maxSize)
        {
            // 아이템 저장 실패
            return false;
        }
        // 위 두 조건 모두 아닐 경우
        // 중복 획득이 가능한 소모품이 아닌 경우 (아이템 획득 시 무조건 새로운 슬롯을 필요로 하는 경우)
        // 인벤토리가 가득 차지 않은 경우 (빈 아이템 슬롯이 존재하는 경우)
        // 인벤토리에 아이템 저장
        InventoryItem newItemInstance = new InventoryItem(newItem, count);
        shopDictionary.Add(newItemInstance.ItemData.ItemID, newItemInstance);
        // UI 갱신
        shopUI.RefreshUI();
        // 아이템 획득 성공
        return true;
    }

    // 아이템 제거 메서드
    public void RemoveItem(int itemID, int count = 1)
    {
        // 예외 처리
        // 상점에 입력된 ID를 가진 아이템이 없다면 리턴
        if (!shopDictionary.ContainsKey(itemID)) return;
        // 입력된 ID를가진 아이템 저장
        InventoryItem targetItem = shopDictionary[itemID];
        // 아이템 수량 감소
        targetItem.SubAmount(count);
        // 남은 아이템 수량이 없다면 제거
        if (targetItem.Amount <= 0)
        {
            shopDictionary.Remove(itemID);
            Debug.Log($"{targetItem.ItemData.ItemName} is removed");
        }
        else
        {
            Debug.Log($"{targetItem.ItemData.ItemName} x{targetItem.Amount}");
        }
        // UI 갱신
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI != null)
        {
            shopUI.RefreshUI();
        }
    }

    // 상점 확장 메서드
    public void ExpandInventory(int amount)
    {
        // 최대 크기 값 증가
        maxSize += amount;
    }

    public bool BuyItem(InventoryItem item, int count = 1)
    {
        // 상점에 입력된 ID를 가진 아이템이 없다면 리턴
        if (!shopDictionary.ContainsKey(item.ItemData.ItemID)) return false;
        // 골드 매니저가 없다면 리턴
        if (GoldManager.Instance == null) return false;
        // 입력된 ID를가진 아이템 저장
        InventoryItem targetItem = shopDictionary[item.ItemData.ItemID];
        // 소지 골드와 비교
        bool isBuyItem = GoldManager.Instance.SpendGold(targetItem.ItemData.ItemPrice);
        // 소지 골드가 부족하다면 리턴
        if (!isBuyItem) return false;
        // 아이템 인벤토리에 추가
        InventoryManager.Instance.AddItem(targetItem.ItemData);
        // 아이템 수량 감소
        targetItem.SubAmount(count);
        // 남은 아이템 수량이 없다면 제거
        if (targetItem.Amount <= 0)
        {
            shopDictionary.Remove(item.ItemData.ItemID);
            Debug.Log($"{targetItem.ItemData.ItemName} is removed");
        }
        else
        {
            Debug.Log($"{targetItem.ItemData.ItemName} x{targetItem.Amount}");
        }
        // UI 갱신
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI != null)
        {
            shopUI.RefreshUI();
        }
        return true;
    }
    // 상점 활성/비활성화 메서드
    public void ToggleShop()
    {
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI == null) return;
        shopUI.ToggleShopUI();
    }
    // 상점이 열려있는지 판단할 메서드
    public bool IsShopOpened()
    {
        ShopUI shopUI = FindFirstObjectByType<ShopUI>();
        if (shopUI == null) return false;
        return shopUI.IsOpened;
    }
}