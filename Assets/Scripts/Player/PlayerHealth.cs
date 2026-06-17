using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStat playerStat;
    private int currentHp;

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        currentHp = playerStat.MaxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage - playerStat.Defense;
        Debug.Log($"플레이어 받은 데미지 {damage - playerStat.Defense} 현재 체력 : {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        currentHp = 0;
        Debug.Log("플레이어 사망");
    }
}
