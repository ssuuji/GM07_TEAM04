using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItemInfoUI : MonoBehaviour
{
    // 출력할 정보 UI 설정
    [Header("Info Panel Setting")]
    [SerializeField] private Image iconImage;                   // 출력할 아이템 아이콘
    [SerializeField] private TextMeshProUGUI nameText;          // 출력할 아이템 이름
    [SerializeField] private TextMeshProUGUI descriptionText;   // 출력할 아이템 설명
    // UI가 생성될 위치 설정
    [Header("Panel Settings")]
    [SerializeField] private Vector2 cursorOffset = new Vector2(15f, -15f);     // 커서 기준 나타날 위치값 조정
    private RectTransform rectTransform;            // UI는 RectTransform으로 위치값을 사용함
    private RectTransform parentRectTransform;      // 부모 위치값 저장용
    private Canvas parentCanvas;                    // 최상위 캔버스 저장용

    private void Awake()
    {
        // 컴포넌트 받아오기
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        // 커서 위치 따라가기
        FollowCursor();
    }

    /*=============== Method ===============*/

    // 아이템 정보 설정
    public void SetItemInfo(InventoryItem item)
    {
        // 아이콘 갱신
        iconImage.sprite = item.ItemData.ItemIcon;
        // 아이템 이름 갱신
        if (item.ItemData is EquippableItem equip)
        {
            nameText.text = $"{equip.ItemName} (+{equip.UpgradeLevel})";
        }
        else
        {
            nameText.text = item.ItemData.ItemName;
        }
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
    // 커서 따라가기 메서드
    private void FollowCursor()
    {
        if (parentRectTransform == null || parentCanvas == null) return; 
        if (Mouse.current == null) return;

        // 캔버스의 렌더 모드가 Overlay면 null, Camera/World면 해당 카메라를 저장
        // 캔버스 상황에 따라 UI 위치에 오차가 생길 수 있기 때문에 미리 설정
        Camera uiCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;
        // InputSystem을 통해 구한 마우스 위치값
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        // 마우스 좌표를 UI 패널 부모의 로컬 좌표로 반환하는 클래스.메서드
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,    // 부모인 캔버스의 RectTransform값
            mousePos,               // InputSystem을 통한 마우스 위치값
            uiCamera,               // 캔버스에 설정되어 있는 RenderMode값
            out Vector2 localPoint  // 결과로 출력될 위치값
        );

        // 출력된 좌표에 오프셋을 더해서 최종 위치 적용
        rectTransform.anchoredPosition = localPoint + cursorOffset;
    }
}