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

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
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

    // 플레이어 상대 방향찾기
    private void SetDirection()
    {
        dir = (target.transform.position.x >= transform.position.x) ? true : false;
    }

    // 추적
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

    // 공격가능 거리판단
    private void RangeMeasure()
    {
        AttackableRange = Mathf.Abs(target.transform.position.x - transform.position.x) < attackRange ? true : false;
    }
}
