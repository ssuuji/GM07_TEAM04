using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStatus playerStatus;
    private bool isDead;
    private bool isInvin; //무적상태

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        if (isInvin) return;

        playerStatus.TakeDamage(damage);

        if (playerStatus.CurrentHp <= 0)
        {
            Die();
        }
    }
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
