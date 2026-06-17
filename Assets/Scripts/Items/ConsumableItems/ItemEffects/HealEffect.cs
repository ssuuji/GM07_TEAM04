using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Data/Item/ConsumableItemEffect/Heal")]

public class HealEffect : ItemEffect
{
    [Header("Effect Setting")]
    [SerializeField] private int healAmount = 10;

    // 프로퍼티
    public int HealAmount => healAmount;

    /*=============== Method ===============*/

    public override void ExecuteEffect(GameObject target)
    {
        // 플레이어의 능력치 스크립트를 받아와 체력 회복 메서드 실행
        Debug.Log($"체력 회복 아이템 사용 시도 ! | {healAmount}");
    }
}