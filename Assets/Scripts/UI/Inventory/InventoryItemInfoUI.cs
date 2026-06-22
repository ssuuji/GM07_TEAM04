using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItemInfoUI : MonoBehaviour
{
    [Header("Info Panel Setting")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Panel Settings")]
    [SerializeField] private Vector2 cursorOffset = new Vector2(15f, -15f);
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        FollowCursor();
    }

    /*=============== Method ===============*/

    public void SetItemInfo(InventoryItem item)
    {
        iconImage.sprite = item.ItemData.ItemIcon;
        nameText.text = item.ItemData.ItemName;

        // 아이템 타입에 따른 설명
        if (item.ItemData.ItemType == ItemType.Equipment)
        {
            if (item.ItemData is WeaponItem weapon)
            {
                descriptionText.text = $"AtkPower +{weapon.AtkPower}";
            }
            else if (item.ItemData is ArmorItem armor)
            {
                descriptionText.text = $"DefPower +{armor.DefPower}";
            }
        }
        if (item.ItemData.ItemType == ItemType.Consumable)
        {
            if (item.ItemData is ConsumableItem consumable)
            {
                if (consumable.Effects == null || consumable.Effects.Count <= 0) return;
                // 여러 효과가 있을 수 있으므로 StringBuilder사용
                // 문자열 최적화
                System.Text.StringBuilder effectDesc = new System.Text.StringBuilder();
                for (int i = 0; i < consumable.Effects.Count; i++)
                {
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
    // 
    private void FollowCursor()
    {
        if (parentRectTransform == null || parentCanvas == null) return; 
        if (Mouse.current == null) return;

        // 캔버스의 렌더 모드가 Overlay면 null, Camera/World면 해당 카메라를 저장
        Camera uiCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        // 마우스 좌표를 UI 패널 부모의 로컬 좌표로 반환하는 법
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,
            mousePos,
            uiCamera,
            out Vector2 localPoint
        );

        // 변환한 좌표에 오프셋을 더해서 최종 위치 적용
        rectTransform.anchoredPosition = localPoint + cursorOffset;
    }
}