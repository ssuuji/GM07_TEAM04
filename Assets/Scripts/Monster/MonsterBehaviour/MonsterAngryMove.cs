using UnityEngine;

public class MonsterAngryMove : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Monster monster;
    [SerializeField] MonsterAttack attack;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float attackRange = 1f;
    private Rigidbody2D rb;
    private bool dir;
    private bool AttackableRange = false;
    public bool IsAttack {  get; private set; } = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        FindDirection();
        RangeMeasure();
        IsAttack = AttackableRange? true : false;
        if(!IsAttack)
        {
            attack.enabled = false;
            Move();
        }
        else
        {
            attack.enabled = true;
        }
    }

    // 플레이어 상대 방향찾기
    private void FindDirection()
    {
        dir = (target.transform.position.x >= transform.position.x) ? true : false;
        monster.SetDirection(dir);
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

    public void OffIsAttack()
    {
        IsAttack = false;
    }
}
