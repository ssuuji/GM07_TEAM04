using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // 관리할 UI 컴포넌트 연결
    // 너무 많은 오브젝트들을 관리하는 것 같기도 하고...
    [Header("UI Component Binding")]
    [SerializeField] private GameObject inventoryUIPanel;               // 인벤토리 UI패널
    [SerializeField] private Transform slotParent;                      // 인벤토리 슬롯 생성 시 들어갈 위치
    [SerializeField] private GameObject slotPrefab;                     // 인벤토리 한 칸을 담당할 슬롯 프리팹
    [SerializeField] private GameObject inventoryGoldUI;                // 인벤토리 골드 UI 오브젝트
    [SerializeField] private GameObject inventoryMainPanel;             // 인벤토리 전체 UI 패널
    [SerializeField] private GameObject InventoryEquipmentUIPanel;      // 인벤토리 장착 아이템 UI 패널
    [SerializeField] private InventoryItemInfoUI inventoryItemInfoUI;   // 인벤토리 아이템 정보 출력 UI
    // UI에 표시될 슬롯들 관리
    [Header("Inventory Slot UI")]
    [SerializeField] private List<InventorySlotUI> slotUIList = new List<InventorySlotUI>();                            // 생성할 SlotUI들 리스트로 관리
    [SerializeField] private List<InventoryEquipmentSlotUI> equipSlotUIList = new List<InventoryEquipmentSlotUI>();     // 장착 중인 아이템 출력 슬롯 UI

    private void Awake()
    {
        // 시작 시 인벤토리 창 끄기
        if (inventoryUIPanel != null)
        {
            inventoryUIPanel.SetActive(false);
        }
        // 골드 출력 UI 끄기
        if (inventoryGoldUI != null)
        {
            inventoryGoldUI.SetActive(false);
        }
        // 아이템 정보 UI 끄기
        if (inventoryItemInfoUI != null)
        {
            inventoryItemInfoUI.gameObject.SetActive(false);
        }
        // 인벤토리 전체 창 (장비창 + 인벤토리 창) 끄기
        if (inventoryMainPanel != null)
        {
            inventoryMainPanel.gameObject.SetActive(false);
        }
        // 장비창 UI 끄기
        if (InventoryEquipmentUIPanel != null)
        {
            InventoryEquipmentUIPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // 시작 시 정해둔 인벤토리 사이즈에 맞게 슬롯 생성
        InitializeInventorySlots(InventoryManager.Instance.MaxSize);
    }

    private void Update()
    {
        // I 키 입력 시 인벤토리 토글
        if (InputManager.IsOpenInventory && !InventoryManager.Instance.IsOtherUIOpened)
        {
            // 인벤토리 토글 메서드
            ToggleInventoryUI();
        }
    }

    /*=============== Method ===============*/
    
    // 장비 슬롯 초기화
    private void InitializeInventorySlots(int amount)
    {
        // 크기 확인 후 이미 슬롯이 매개변수만큼 있으면 리턴
        int currnetCount = slotUIList.Count;
        if (currnetCount >= amount) return;
        // 생성할 슬롯 칸 수 확인 (장비 칸 증가 대비)
        int createSlotCount = amount - currnetCount;
        // 슬롯 초기화
        for (int i = 0; i < createSlotCount; i++)
        {
            // 슬롯을 생성해 부모 오브젝트 밑으로 저장
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            // 생성한 오브젝트에서 스크립트 받아오기
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            if (slotUI == null) return;
            // 슬롯을 빈 칸으로 초기화
            slotUI.ClearSlot();
            // Slot에서 담당하던 메서드 이벤트를 통해 전달받아 실행
            slotUI.Initialize(
                onClick: (clickedItem) =>                               // 슬롯 클릭 시 호출
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        // 실제 아이템 사용 호출
                        clickedItem.ItemData.Use(player, clickedItem.ItemData.ItemID);
                    }
                    // UI 갱신
                    RefreshUI();
                },
                onEnter: (hoveredItem) => ShowItemInfo(hoveredItem),    // 슬롯 위로 커서가 올라갈 때 호출
                onExit: () => CloseItemInfo()                           // 슬롯에서 커서가 나갈 때 호출
            );
            // 인벤토리 슬롯 UI 관리 List에 추가
            slotUIList.Add(slotUI);
        }
        // 장착 아이템 수량 체크
        int equipmentCount = equipSlotUIList.Count;
        // 이미 장착중이라면 리턴
        if (equipmentCount >= amount) return;
        // 장착 슬롯 리스트 순회
        for (int i = 0; i < equipmentCount; i++)
        {
            // 슬롯이 비엇다면 리턴
            if (equipSlotUIList[i] == null) return;
            // 슬롯 초기화
            equipSlotUIList[i].ClearSlot();
            // 슬롯의 부모 UI 스크립트 설정
            equipSlotUIList[i].Initialize(this);
        }
    }
    // 인벤토리 UI 활성/비활성화
    private void ToggleInventoryUI()
    {
        // 예외 처리
        if (inventoryUIPanel == null) return;
        if (inventoryGoldUI == null) return;
        // 인벤토리 패널이 켜져있는지 검사
        bool isActive = inventoryUIPanel.activeSelf;
        // 인벤토리 관련 UI들의 활성화 상태를 현 상태의 반대 상태로 전환
        inventoryMainPanel.SetActive(!isActive);
        inventoryUIPanel.SetActive(!isActive);
        inventoryGoldUI.SetActive(!isActive);
        InventoryEquipmentUIPanel.SetActive(!isActive);
        InventoryManager.Instance.ThisUICheck(!isActive);

        if (!isActive)
        {
            // 인벤토리가 켜질 때 데이터 갱신
            RefreshUI();
            InventoryManager.Instance.ThisUICheck(!isActive);
        }
        else
        {
            // 인벤토리 꺼질 때 아이템 정보 UI도 끄기
            CloseItemInfo();
        }
    }
    // 인벤토리 데이터 갱신
    public void RefreshUI()
    {
        // 가방 확장 시 슬롯 추가
        if (slotUIList.Count < InventoryManager.Instance.MaxSize)
        {
            // 확장을 했으니 늘어난 크기로 인벤토리 슬롯 초기화
            InitializeInventorySlots(InventoryManager.Instance.MaxSize);
        }
        // 모든 슬롯 데이터 초기화
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].ClearSlot();
        }
        // 인벤토리 슬롯에 실제 아이템 데이터 저장
        int slotIndex = 0;
        foreach (var item in InventoryManager.Instance.InventoryDictionary)
        {
            // 인벤토리가 가득 찬 경우 중지
            if (slotIndex >= slotUIList.Count) break;
            // 아이템 생성 및 저장
            InventoryItem itemInstance = item.Value;
            // 슬롯에 아이템 추가
            slotUIList[slotIndex].UpdateSlot(itemInstance);
            // 다음 인덱스로
            slotIndex++;
        }
        // 장착 슬롯 처리
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // 플레이어의 장착 정보 가져오기
            PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();
            if (equipment != null)
            {
                // 슬롯 별 아이템 데이터 갱신
                // 수가 많아지면 반복문을 사용하는 편이 좋을 것 같다.
                if (equipSlotUIList[0] != null) equipSlotUIList[0].UpdateEquipmentSlot(equipment);
                if (equipSlotUIList[1] != null) equipSlotUIList[1].UpdateEquipmentSlot(equipment);
            }
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
}