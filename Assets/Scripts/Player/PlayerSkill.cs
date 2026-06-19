using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    //플레이어 스킬 해금 담당

    [Header("스킬 해금 상태")]
    [SerializeField] private bool areaAttackUnlocked; // X 범위공격
    [SerializeField] private bool buffUnlocked;       // C 공격버프
    [SerializeField] private bool invinUnlocked;      // V 무적기

    public bool AreaAttackUnlocked => areaAttackUnlocked;
    public bool BuffUnlocked => buffUnlocked;
    public bool InvinUnlocked => invinUnlocked;     

    //범위공격 스킬 해금
    public void UnlockAreaAttack()
    {
        if (areaAttackUnlocked) return;

        areaAttackUnlocked = true;

        Debug.Log("범위공격(X) 스킬 해금");
    }

    //공격버프 스킬 해금
    public void UnlockBuff()
    {
        if (buffUnlocked) return;

        buffUnlocked = true;

        Debug.Log("공격버프(C) 스킬 해금");
    }

    //무적기 스킬 해금
    public void UnlockInvin()
    {
        if (invinUnlocked) return;

        invinUnlocked = true;

        Debug.Log("무적기 스킬 해금");
    }

}
