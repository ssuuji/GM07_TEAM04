using UnityEngine;

public class Item : ScriptableObject
{
    // 아이템 내부 데이터
    [Header("Item Data")]
    [SerializeField] private int itemID;            // 아이템 분류 ID
    [SerializeField] private ItemType itemType;     // 아이템 종류 ( 장비, 소모품, ...)
    [SerializeField] private GameObject itemPrefab; // 아이템 프리팹
    // UI에 표기할 아이템 정보
    [Header("Item Info")]
    [SerializeField] private string itemName;       // 아이템 이름
    [SerializeField] private Sprite itemIcon;       // 아이템 아이콘
    [SerializeField] private int maxAmount;         // 아이템 최대 소지 개수

    // 출력 메세지
    [Header("Message")]
    [TextArea(2, 15)]
    private string messageWhenCollected;            // 아이템 획득 시 출력할 메세지
    [TextArea(2, 15)]
    private string messageWhenUsed;                 // 아이템 사용 시 출력할 메세지

    // 프로퍼티
    // 데이터
    public int ItemID => itemID;
    public ItemType ItemType => itemType;
    public GameObject ItemPrefab => itemPrefab;
    // 정보
    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;
    public int MaxAmount => maxAmount;
    // 메세지
    public string MessageWhenCollected => messageWhenCollected;
    public string MessageWhenUsed => messageWhenUsed;

    /*=============== Method ===============*/
    
    // 상속받은 객체들 사용 기능 작성할 메서드
    public virtual void Use(GameObject target, int itemID)
    {
        Debug.Log($"{itemName} 사용을 시도했습니다.");
    }
    // 아이템 타입 세팅
    protected void SetItemType(ItemType type)
    {
        this.itemType = type;
    }
}