using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Data/Item/ConsumableItemEffect/Heal")]

public class HealEffect : ItemEffect
{
    [Header("Effect Setting")]
    [SerializeField] private int healAmount = 10;

    // 프로퍼티
    public int HealAmount => healAmount; 
    public override string Description => $"HP +{healAmount}";    // 아이템 효과 점보 프로퍼티 설정

    /*=============== Method ===============*/

    public override bool ExecuteEffect(GameObject target)
    {
        // 플레이어의 능력치 스크립트를 받아와 체력 회복 메서드 실행
        Debug.Log($"체력 회복 아이템 사용 시도 ! | {healAmount}");

        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus == null) return false;
        if (playerStatus.CurrentHp >= playerStatus.CurrentMaxHp)
        {
            // 체력이 가득 찼다는 디버그
            Debug.Log("이미 체력이 가득 차 있어 사용할 수 없습니다.");
            MessageUI message = FindFirstObjectByType<MessageUI>();
            message.ShowMessage("이미 체력이 가득 차 있어 사용할 수 없습니다.");
            return false;
        }
        playerStatus.Heal(healAmount);
        return true;
    }
}