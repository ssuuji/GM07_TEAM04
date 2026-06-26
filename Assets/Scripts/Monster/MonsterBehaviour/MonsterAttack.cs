using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCoolTime = 1f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(2f, 1f);
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Monster monster;

    [SerializeField] private DogAnimation dogAnimation;

    private float currentCoolTime;

    private void Awake()
    {
        currentCoolTime = attackCoolTime;
    }

    private void Update()
    {
        if (currentCoolTime <= 0)
        {
            Attack();
            
            currentCoolTime = attackCoolTime;
        }

        else
        {
            currentCoolTime -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        Debug.Log("몬스터의 Attack");
        SetAttackDirection();
        dogAnimation.Attack();
        Collider2D player = Physics2D.OverlapBox(
            attackPoint.position,
            attackSize,
            0f,
            playerLayer);

        if (player != null)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    public void SetAttackDirection()
    {
        if (!monster.Direction)
        {
            attackPoint.localPosition = new Vector3(-1f, 0, 0f);
        }
        else
        {
            attackPoint.localPosition = new Vector3(1f, 0f, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}
