using UnityEngine;

[CreateAssetMenu(fileName = "NewGoldItem", menuName = "Data/Item/Gold")]

public class GoldItem : Item
{
    [Header("Gold Amount")]
    [SerializeField] private int goldAmount = 10;   // 아이템 획득 시 얻을 골드량

    // 프로퍼티
    public int GoldAmount => goldAmount;

    private void Awake()
    {
        SetItemType(ItemType.Gold);
    }

    /*=============== Method ===============*/

    public override void Use(GameObject target, int itemID)
    {
        // 골드는 획득 시 사용 기능이 필요 없음
    }
}