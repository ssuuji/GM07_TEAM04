using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    // 관리할 UI 컴포넌트 연결
    [Header("UI Component Binding")]
    [SerializeField] private GameObject inventoryUIPanel;   // 인벤토리 전체 UI패널
    [SerializeField] private Transform slotParent;          // 인벤토리 슬롯 생성 시 들어갈 위치
    [SerializeField] private GameObject slotPrefab;         // 인벤토리 한 칸을 담당할 슬롯 프리팹
    // 골드 등이 들어온다면 여기에 추가

    private List<InventorySlotUI> slotUIList = new List<InventorySlotUI>(); // 생성할 SlotUI들 리스트로 관리

    private void Awake()
    {
        // 시작 시 인벤토리 창 끄기
        if (inventoryUIPanel != null)
        {
            inventoryUIPanel.SetActive(false);
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
        if (InputManager.IsOpenInventory)
        {
            // 인벤토리 토글 메서드
            ToggleInventoryUI();
        }
    }

    /*=============== Method ===============*/
    
    private void InitializeInventorySlots(int amount)
    {
        // 크기 확인 후 이미 슬롯이 매개변수만큼 있으면 리턴
        int currnetCount = slotUIList.Count;
        if (currnetCount >= amount) return;
        // 생성할 슬롯 칸 수 확인
        int createSlotCount = amount - currnetCount;
        // 슬롯 초기화
        for (int i = 0; i < createSlotCount; i++)
        {
            // 슬롯을 생성해 부모 오브젝트 밑으로 저장
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            // 생성한 오브젝트에서 스크립트 받아오기
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            // 예외 처리
            if (slotUI == null) return;
            // 슬롯을 빈 칸으로 초기화 후 해당 위치에 아이템 저장
            slotUI.ClearSlot();
            slotUIList.Add(slotUI);
        }
    }
    private void ToggleInventoryUI()
    {
        // 예외 처리
        if (inventoryUIPanel == null) return;
        // 인벤토리 패널이 켜져있는지 검사
        bool isActive = inventoryUIPanel.activeSelf;
        // 인벤토리의 현 상태의 반대 상태로 전환
        inventoryUIPanel.SetActive(!isActive);

        if (!isActive)
        {
            // 인벤토리가 닫히면 데이터 갱신
        }
    }
    // 인벤토리 데이터 갱신
    public void RefreshUI()
    {
        // 가방 확장 시 슬롯 추가
        if (slotUIList.Count < InventoryManager.Instance.MaxSize)
        {
            InitializeInventorySlots(InventoryManager.Instance.MaxSize);
        }
        // 모든 슬롯 초기화
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].ClearSlot();
        }
        // 인벤토리 슬롯에 실제 아이템 데이터 저장
        int slotIndex = 0;
        foreach (var item in InventoryManager.Instance.InventoryDictionary)
        {
            // 예외 처리
            // 인벤토리가 가득 찬 경우 중지
            if (slotIndex >= slotUIList.Count) break;
            // 슬롯에 아이템 저장
            InventoryItem itemInstance = item.Value;
            slotUIList[slotIndex].UpdateSlot(itemInstance);
            // 다음 인덱스로
            slotIndex++;
        }
    }
}