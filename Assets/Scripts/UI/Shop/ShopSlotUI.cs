using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Data Binding")]
    [SerializeField] private Image iconImage;                   // 아이템 아이콘에 나타날 이미지
    [SerializeField] private TextMeshProUGUI amountText;        // 아이템 수량을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI nameText;          // 아이템 이름 텍스트
    [SerializeField] private TextMeshProUGUI priceText;         // 아이템 가격 텍스트
    [SerializeField] private TextMeshProUGUI descriptionText;   // 아이템 정보 텍스트

    // UI 칸에서 현재 보여지고 있는 아이템의 실제 데이터
    private InventoryItem currentItem;

    /*=============== Method ===============*/

    public void UpdateSlot(InventoryItem item)
    {
        // 아이템 데이터 저장
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
        // 1개 이하면 비활성화
        else
        {
            amountText.gameObject.SetActive(false);
        }
        // 아이템 이름 활성화 및 갱신
        nameText.gameObject.SetActive(true);
        nameText.text = item.ItemData.ItemName;
        // 아이템 가격 활성화 및 갱신
        priceText.gameObject.SetActive(true);
        priceText.text = item.ItemData.ItemPrice.ToString();
        // 아이템 설명 활성화
        descriptionText.gameObject.SetActive(true);
        // 아이템 타입에 따른 설명 갱신
        if (item.ItemData.ItemType == ItemType.Equipment)
        {
            if (item.ItemData is WeaponItem weapon)
            {
                descriptionText.text = $"AtkPower +{weapon.CurrentAtkPower}";
            }
            else if (item.ItemData is ArmorItem armor)
            {
                descriptionText.text = $"DefPower +{armor.CurrentDefPower}";
            }
        }
        if (item.ItemData.ItemType == ItemType.Consumable)
        {
            if (item.ItemData is ConsumableItem consumable)
            {
                if (consumable.Effects == null || consumable.Effects.Count <= 0) return;
                // 여러 효과가 있을 수 있으므로 StringBuilder사용
                // StringBuilder : 문자열 수정, 문자열 결합 시 발생하는 메모리 할당, GC 문제를 방지해 최적화하는 클래스
                System.Text.StringBuilder effectDesc = new System.Text.StringBuilder();
                for (int i = 0; i < consumable.Effects.Count; i++)
                {
                    // Effect에 정의해둔 Description 활용
                    effectDesc.Append(consumable.Effects[i].Description);

                    // 효과가 여러 개일 경우 줄바꿈 추가
                    if (i < consumable.Effects.Count - 1)
                    {
                        effectDesc.AppendLine();
                    }
                }
                // 최종 텍스트 적용
                descriptionText.text = effectDesc.ToString();
            }
        }
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
        descriptionText.gameObject.SetActive(false);
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