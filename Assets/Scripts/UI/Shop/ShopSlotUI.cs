using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;               // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;    // 아이템 수량을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI nameText;      // 아이템 이름 텍스트
    [SerializeField] private TextMeshProUGUI priceText;     // 아이템 가격 텍스트

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    /*=============== Method ===============*/

    public void UpdateSlot(InventoryItem item)
    {
        currentItem = item;
        // 아이콘 이미지 적용 및 투명도 조절
        iconImage.sprite = item.ItemData.ItemIcon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // 아이템 수량 표시
        // 중복 아이템이 2개 이상부터 표시
        if (item.Amount > 1)
        {
            amountText.text = item.Amount.ToString();
            amountText.gameObject.SetActive(true);
        }
        else
        {
            amountText.gameObject.SetActive(false);
        }
        nameText.gameObject.SetActive(true);
        nameText.text = item.ItemData.ItemName;
        priceText.gameObject.SetActive(true);
        priceText.text = item.ItemData.ItemPrice.ToString();
    }
    // 슬롯 초기화 메서드
    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        amountText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);
    }
    // 유니티 제공 마우스 클릭 이벤트 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        // 예외 처리
        // 빈 칸 클릭 시 리턴
        if (currentItem == null) return;

        // 마우스 좌클릭
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("LeftClick");

            // 아이템 구매 메서드 호출
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            ShopManager.Instance.BuyItem(currentItem);

            // UI 갱신
            ShopUI shopyUI = FindFirstObjectByType<ShopUI>();
            if (shopyUI == null) return;
            shopyUI.RefreshUI();
        }
        // 마우스 우클릭
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RightClick");
        }
    }
}