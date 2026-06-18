using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackableRange = 3f;
    [SerializeField] private float attackCoolTime = 2f;
    [SerializeField] private LayerMask playerLayer;

    private Vector2 direction;

    public void Attack()
    {
        if(attackCoolTime<= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            attackableRange,
            playerLayer
            );

            if (hit.collider != null)
            {
                PlayerHealth player = hit.collider.GetComponent<PlayerHealth>();

                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                }
            }
        }

        else
        {
            attackCoolTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            transform.position,
            direction * attackableRange);
    }

    public void SetAttackDirection(bool dir)
    {
        direction = dir ? Vector2.right : Vector2.left;
    }
}
