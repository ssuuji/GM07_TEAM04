using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 마우스 클릭 이벤트를 위한 상속

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;               // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;    // 아이템 수량을 표시할 텍스트
    [SerializeField] private GameObject equipMark;          // 장비 아이템의 장착 여부 표시 마크

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    // 부모 UI 설정
    private InventoryUI inventoryUI;

    /*=============== Method ===============*/

    // 인벤토리 부모 UI 설정
    // 메서드로 부모를 알려주는 방식이 하나하나 Find로 컴포넌트를 찾는 방식보다 효율적이라고 한다.
    public void Initialize(InventoryUI parent)
    {
        inventoryUI = parent;
    }
    // 슬롯 내부 데이터 갱신 메서드
    public void UpdateSlot(InventoryItem item)
    {
        // 새로 들어온 아이템 저장
        currentItem = item;
        // 아이콘 이미지 적용 및 투명도 조절
        iconImage.sprite = item.ItemData.ItemIcon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // 아이템 수량 표시
        // 중복 아이템이 2개 이상부터 표시
        if (item.Amount > 1)
        {
            // 소지 수량 string으로 변환해 text 내용 변환
            amountText.text = item.Amount.ToString();
            // 수량 오브젝트 켜기
            amountText.gameObject.SetActive(true);
        }
        // 한 개만 가지고 있다면
        else
        {
            // 수량 오브젝트 끄기
            amountText.gameObject.SetActive(false);
        }
        // 장비 아이템이라면 장착 여부 표시
        if (equipMark == null) return;
        if (item.ItemData.ItemType == ItemType.Equipment)
        {
            // 장착 여부 정보를 플레이어 장착 관리 스크립트에서 받아와 조건 부 실행
            PlayerEquipment playerEquipment = FindFirstObjectByType<PlayerEquipment>();
            if (playerEquipment == null) return;
            // 장착 여부 판단 변수
            bool isEquipped = false;
            // 무기 비교 (무기가 장착되어 있고, 장착되어 있는 아이템의 ID값이 검사하는 슬롯의 아이템 ID값과 같다면 true)
            if (playerEquipment.WeaponItem != null && playerEquipment.WeaponItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }
            // 방어구 비교 (방어구가 장착되어 있고,장착되어  있는 아이템의 ID값이 검사하는 슬롯의 아이템 ID값과 같다면 true)
            if (playerEquipment.ArmorItem != null && playerEquipment.ArmorItem.ItemID == item.ItemData.ItemID)
            {
                isEquipped = true;
            }
            // 최종 결과에 따라 마크 활성화/비활성화
            equipMark.SetActive(isEquipped);
        }
        else
        {
            // 장비가 아닌 아이템들은 장착 마크 비활성화
            equipMark.SetActive(false);
        }
    }
    // 슬롯 초기화 메서드
    public void ClearSlot()
    {
        // 다 비우고 끄고 투명하게
        currentItem = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        amountText.gameObject.SetActive(false);
        if (equipMark == null) return;
        equipMark.SetActive(false);
    }
    // 유니티 제공 마우스 클릭 이벤트 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        // 빈 칸 클릭 시 리턴
        if (currentItem == null) return;
        // 마우스 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 클릭이 됐는지 확인 디버그
            Debug.Log("LeftClick");
            // 플레이어 정보 받아오기
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            // 아이템 사용 메서드 호출
            currentItem.ItemData.Use(player, currentItem.ItemData.ItemID);
            // 인벤토리 UI 갱신
            InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
            if (inventoryUI == null) return;
            inventoryUI.RefreshUI();
        }
        // 마우스 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 클릭이 됐는지 확인 메서드
            Debug.Log("RightClick");
        }
    }
    // 마우스가 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 빈 칸일 시 리턴
        if (currentItem == null) return;
        // 아이템 정보 UI 활성화
        inventoryUI.ShowItemInfo(currentItem);
    }
    // 마우스가 떠났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        // 아이템 정보 UI 비활성화
        inventoryUI.CloseItemInfo();
    }
}