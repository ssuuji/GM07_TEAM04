using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    // 관리할 UI 컴포넌트 연결
    [Header("UI Component Binding")]
    [SerializeField] private GameObject shopUIPanel;        // 상점 전체 UI패널
    [SerializeField] private Transform slotParent;          // 상점 슬롯 생성 시 들어갈 위치
    [SerializeField] private GameObject slotPrefab;         // 상점 한 칸을 담당할 슬롯 프리팹
    [SerializeField] private GameObject goldUI;             // 골드 UI 오브젝트

    private List<ShopSlotUI> slotUIList = new List<ShopSlotUI>(); // 생성할 SlotUI들 리스트로 관리

    // 프로퍼티
    public bool IsOpened => shopUIPanel != null && shopUIPanel.activeSelf;  // UI가 존재하고 켜져있을 때

    private void Awake()
    {
        // 시작 시 상점 UI 끄기
        if (shopUIPanel != null)
        {
            shopUIPanel.SetActive(false);
        }
        // 골드 출력 UI 끄기
        if (goldUI != null)
        {
            goldUI.SetActive(false);
        }
    }

    private void Start()
    {
        // 상점 가방 초기화
        InitializeShopSlots(ShopManager.Instance.MaxSize);
    }

    /*=============== Method ===============*/

    // 초기 상점 세팅
    private void InitializeShopSlots(int amount)
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
            ShopSlotUI slotUI = slotObj.GetComponent<ShopSlotUI>();
            // 예외 처리
            if (slotUI == null) return;
            // 슬롯을 빈 칸으로 초기화 후 해당 위치에 아이템 저장
            slotUI.ClearSlot();
            slotUIList.Add(slotUI);
        }
    }
    public void ToggleShopUI()
    {
        // 예외 처리
        if (shopUIPanel == null) return;
        if (goldUI == null) return;
        // 인벤토리 패널이 켜져있는지 검사
        bool isActive = shopUIPanel.activeSelf;
        // 인벤토리의 현 상태의 반대 상태로 전환
        shopUIPanel.SetActive(!isActive);
        goldUI.SetActive(!isActive);
        InventoryManager.Instance.OtherUICheck(!isActive);

        if (!isActive)
        {
            // 인벤토리가 켜질 때 데이터 갱신
            RefreshUI();
            InventoryManager.Instance.OtherUICheck(!isActive);
        }
    }
    // 상점 데이터 갱신
    public void RefreshUI()
    {
        // 가방 확장 시 슬롯 추가
        if (slotUIList.Count < ShopManager.Instance.MaxSize)
        {
            InitializeShopSlots(ShopManager.Instance.MaxSize);
        }
        // 모든 슬롯 초기화
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].ClearSlot();
        }
        // 상점 슬롯에 실제 아이템 데이터 저장
        int slotIndex = 0;
        foreach (var item in ShopManager.Instance.ShopDictionary)
        {
            // 상점이 가득 찬 경우 중지
            if (slotIndex >= slotUIList.Count) break;
            // 슬롯에 아이템 저장
            InventoryItem itemInstance = item.Value;
            slotUIList[slotIndex].UpdateSlot(itemInstance);
            // 다음 인덱스로
            slotIndex++;
        }
    }
}