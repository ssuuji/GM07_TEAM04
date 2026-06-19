using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStatus playerStatus;
    private bool isDead;  //플레이어 사망여부
    private bool isInvin; //무적상태 확인여부

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    //데미지 받을 때
    public void TakeDamage(int damage)
    {
        if (isDead) return;  //이미 사망상태면 X
        if (isInvin) return; //무적상태면 X

        //damage만큼 HP 감소
        playerStatus.TakeDamage(damage);

        //HP가 0이하라면 사망처리(Die)
        if (playerStatus.CurrentHp <= 0)
        {
            Die();
        }
    }

    //플레이어 사망처리
    private void Die()
    {
        isDead = true;
        Debug.Log("플레이어 사망");
    }

    //무적 상태 변경
    public void SetInvin(bool value)
    {
        isInvin = value;
    }
}
