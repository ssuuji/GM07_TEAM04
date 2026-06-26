using UnityEngine;

[CreateAssetMenu(fileName = "ManaRecoveryEffect", menuName = "Data/Item/ConsumableItemEffect/ManaRecovery")]

public class ManaRecoveryEffect : ItemEffect
{
    [Header("Effect Setting")]
    [SerializeField] private int manaRecoveryAmount = 10;

    // 프로퍼티
    public int ManaRecoveryAmount => manaRecoveryAmount;
    public override string Description => $"HP +{manaRecoveryAmount}";    // 아이템 효과 점보 프로퍼티 설정

    /*=============== Method ===============*/

    public override bool ExecuteEffect(GameObject target)
    {
        // 플레이어의 능력치 스크립트를 받아와 체력 회복 메서드 실행
        Debug.Log($"마나 회복 아이템 사용 시도 ! | {manaRecoveryAmount}");

        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus == null) return false;
        if (playerStatus.CurrentMp >= playerStatus.CurrentMaxMp)
        {
            // 체력이 가득 찼다는 디버그
            Debug.Log("이미 마나가 가득 차 있어 사용할 수 없습니다.");
            return false;
        }
        playerStatus.RecoverMp(manaRecoveryAmount);
        return true;
    }
}