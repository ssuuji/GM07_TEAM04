using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStatus playerStatus;
    private bool isDead;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

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
}
