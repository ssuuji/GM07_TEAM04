using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmithUI : Singleton<SmithUI>
{
    // 수많은 UI 연결...
    [Header("UI Component Binding")]
    [SerializeField] private GameObject smithUI;                        // 강화 전체 UI
    [SerializeField] private GameObject smithUIPanel;                   // 강화 대상 아이템 표시될 UI
    [SerializeField] private GameObject smithInventoryUIPanel;          // 소지한 장비 아이템 표시할 UI
    [SerializeField] private Transform slotParent;                      // 소지한 장비 아이템 슬롯들이 들어갈 부모
    [SerializeField] private GameObject slotPrefab;                     // 슬롯 프리팹
    [SerializeField] private InventoryItemInfoUI inventoryItemInfoUI;   // 아이템 정보 출력 UI
    [SerializeField] private SmithSlotUI upgradeSlot;                   // 선택된 강화할 아이템 표시할 슬롯
    [SerializeField] private Button upgradeButton;                      // 강화를 진행할 버튼
    [SerializeField] private TextMeshProUGUI itemUpgradePriceText;      // 강화 소모 재화 표시 텍스트

    [Header("Slot UI")]
    [SerializeField] private List<InventorySlotUI> slotUIList = new List<InventorySlotUI>();    // 소지한 장비 아이템 표시할 슬롯들을 관리할 List

    // 프로퍼티
    public bool IsOpened => smithUI != null && smithUI.activeSelf;
    protected override void Awake()
    {
        // UI 전부 닫고 시작
        ClearUI();
    }

    private void Start()
    {
        // 슬롯들 초기 생성
        InitializeSmithSlots(InventoryManager.Instance.MaxSize);
        // 연결한 Button UI에 메서드 연결
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        }
    }

    /*=============== Method ===============*/
    
    // 장비 슬롯 초기화 (가방이 처음 만들어지거나 크기가 달라질 때 실행)
    private void InitializeSmithSlots(int amount)
    {
        // 생성할 슬롯 개수
        int currentCount = slotUIList.Count;
        if (currentCount >= amount) return;

        // 부족한 칸 수만큼 슬롯 추가 생성 (가방 확장 대응)
        int createSlotCount = amount - currentCount;
        for (int i = 0; i < createSlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                // 대장간 전용 콜백
                slotUI.Initialize(
                    onClick: (clickedItem) => OnSelectUpgradeItem(clickedItem), // 클릭 시 아이템 선택 메서드 호출
                    onEnter: (hoveredItem) => ShowItemInfo(hoveredItem),        // 커서 호버 시 정보 UI 출력
                    onExit: () => CloseItemInfo()                               // 커서가 나갈 시 정보 UI 제거
                );
                // 슬롯 초기화
                slotUI.ClearSlot();
                // 슬롯에 아이템 추가
                slotUIList.Add(slotUI);
            }
        }
    }
    // 대장간 UI 활성/비활성화 메서드
    public void ToggleSmithUI()
    {
        if (smithUI == null) return;
        // 인벤토리 패널이 켜져있는지 검사
        bool isActive = smithUI.activeSelf;
        // 인벤토리의 현 상태의 반대 상태로 전환
        smithUI.SetActive(!isActive);
        smithUIPanel.SetActive(!isActive);
        smithInventoryUIPanel.SetActive(!isActive);
        itemUpgradePriceText.gameObject.SetActive(!isActive);

        if (!isActive)
        {
            // UI가 켜질 때 데이터 갱신
            RefreshUI();
        }
        else
        {
            // UI가 닫힐 때 끌 것들
            CloseItemInfo();
            if (upgradeSlot != null)
            {
                upgradeSlot.ClearSlot();
            }
        }
    }
    // 인벤토리 데이터 갱신
    public void RefreshUI()
    {
        // 가방 확장 시 슬롯 추가
        if (slotUIList.Count < InventoryManager.Instance.MaxSize)
        {
            InitializeSmithSlots(InventoryManager.Instance.MaxSize);
        }
        // 모든 슬롯 데이터 초기화
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].ClearSlot();
        }
        // 인벤토리 슬롯에 실제 아이템 데이터 저장
        List<InventoryItem> equipmentList = new List<InventoryItem>();
        foreach (var item in InventoryManager.Instance.InventoryDictionary.Values)
        {
            if (item.ItemData.ItemType == ItemType.Equipment)
            {
                equipmentList.Add(item);
            }
        }
        // 생성된 가방 크기만큼 순회
        for (int i = 0; i < slotUIList.Count; i++)
        {
            // 인벤토리에 장비 아이템이 있다면 순서대로 저장
            if (i < equipmentList.Count)
            {
                slotUIList[i].UpdateSlot(equipmentList[i]);
            }
        }
        // 강화 소모 재화 수정
        if (upgradeSlot.TargetItem == null) return;
        if (upgradeSlot.TargetItem.ItemData is EquippableItem equip)
        {
            itemUpgradePriceText.text = $"{100 + (equip.UpgradeLevel * 100)} G";
        }
    }
    // 아이템 정보 UI 출력
    public void ShowItemInfo(InventoryItem item)
    {
        if (item == null) return;
        // 아이템 정보 출력용 UI에 아이템 데이터 갱신
        inventoryItemInfoUI.SetItemInfo(item);
        // UI 활성화
        inventoryItemInfoUI.gameObject.SetActive(true);
    }
    // 아이템 정보 UI 끄기
    public void CloseItemInfo()
    {
        // UI 비활성화
        inventoryItemInfoUI.gameObject.SetActive(false);
    }
    // 슬롯 클릭 시 실행될 메서드
    private void OnSelectUpgradeItem(InventoryItem selectedItem)
    {
        if (selectedItem == null || selectedItem.ItemData == null) return;
        if (selectedItem.ItemData.ItemType != ItemType.Equipment) return;
        if (upgradeSlot == null) return;

        Debug.Log($"강화 대상으로 선택된 장비: {selectedItem.ItemData.ItemName}");
        // 강화 슬롯에 선택된 아이템 전달
        upgradeSlot.SelectedItem(selectedItem);
    }
    // 업그레이드 버튼이 눌렸을 때 실행될 메서드
    public void OnClickUpgradeButton()
    {
        if (upgradeSlot == null || upgradeSlot.TargetItem == null) return;
        // 슬롯의 아이템 저장
        InventoryItem currentItem = upgradeSlot.TargetItem;
        // 장비 아이템으로 캐스팅
        if (currentItem.ItemData is EquippableItem equipItem)
        {
            // 강화 메서드 실행
            equipItem.UpgradeItem();
            // 강화된 아이템으로 선택 아이템 설정
            upgradeSlot.SelectedItem(currentItem);
            // UI 갱신
            RefreshUI();
        }
    }
    // 전부 닫기
    private void ClearUI()
    {
        // 열려있다면 시작할 때 전부 닫고 시작
        if (smithUI != null)
        {
            smithUI.SetActive(false);
        }
        if (smithUIPanel != null)
        {
            smithUIPanel.SetActive(false);
        }
        if (smithInventoryUIPanel != null)
        {
            smithInventoryUIPanel.SetActive(false);
        }
        if (inventoryItemInfoUI != null)
        {
            inventoryItemInfoUI.gameObject.SetActive(false);
        }
        if (itemUpgradePriceText != null)
        {
            itemUpgradePriceText.gameObject.SetActive(false);
        }
    }
}