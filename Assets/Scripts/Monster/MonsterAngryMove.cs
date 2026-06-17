using UnityEngine;

public class MonsterAngryMove : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float attackRange = 1f;

    private Rigidbody2D rb;
    private bool dir;
    private bool AttackableRange = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetDirection();
        RangeMeasure();
        if(AttackableRange)
        {
            //공격
        }
        else
        {
            Move();
        }
    }

    private void SetDirection()
    {
        dir = (target.transform.position.x >= transform.position.x) ? true : false;
    }

    private void Move()
    {
        if (dir)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }
    }

    private void RangeMeasure()
    {
        AttackableRange = Mathf.Abs(target.transform.position.x - transform.position.x) < attackRange ? true : false;
    }
}
