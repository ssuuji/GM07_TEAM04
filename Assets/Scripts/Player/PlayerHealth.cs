using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerBaseStatSO playerStat;
    private PlayerEquipment playerEquipment;
    private int currentHp;
    private bool isDead;

    private void Start()
    {
        playerEquipment = GetComponent<PlayerEquipment>();
        currentHp = playerStat.MaxHp;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int takeDamage = damage - playerEquipment.CurrentDefense;

        //방어력이 데미지 보다 높은 경우엔 데미지 1만 받도록
        if (takeDamage < 1)
        {
            takeDamage = 1;
        }
        currentHp -= takeDamage;
        Debug.Log($"플레이어 받은 데미지 {takeDamage} 현재 체력 : {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        currentHp = 0;
        Debug.Log("플레이어 사망");
    }
}
