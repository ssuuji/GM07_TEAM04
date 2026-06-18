using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void Attack()
    {
        //기본공격
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);
        if (hit == null) return;

        IDamageable damageable = hit.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(playerStatus.CurrentAttack);
        }
        Debug.Log($"{hit.name} 공격 | 데미지 {playerStatus.CurrentAttack}");

        // X C V 관련 스킬공격 추가하기

    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
