using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpPower = 8.0f;
    private Rigidbody2D rb;
    private float originGravity;
    private Vector3 originScale;

    [Header("바닥 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.08f);
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;

    [Header("점프 설정")]
    [SerializeField] private int maxJumpCount = 2;
    private int jumpcount;

    [Header("대쉬 설정")]
    [SerializeField] private float dashPower = 10.0f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;
    private bool isDash;         //대쉬중
    private bool canDash = true; //대쉬 사용가능 여부
    private float checkDir = 1f; //대쉬 방향

    [Header("공격 설정")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    private PlayerStat playerStat;

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale;
        originScale = transform.localScale;
    }
    private void Update()
    {
        CheckGround(); //바닥체크
        CheckDir();    //대쉬 방향체크

        if (InputManager.IsJump)
        {
            Jump();
        }
        if (InputManager.IsDash && canDash)
        {
            Dash();
        }
        if (InputManager.IsBasicAttack)
        {
            Attack();
        }
        if (InputManager.IsInteract)
        {
            Debug.Log("상호작용");
        }

    }
    private void FixedUpdate()
    {
        //대쉬 속도가 Move()에 의해 덮어지는 것을 방지
        if (isDash) return; 

        Move();
    }
    private void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);
        if (hit == null) return;

        IDamageable damageable = hit.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(playerStat.Attack);
        }
        Debug.Log($"{hit.name} 공격");
    }
    private void Dash()
    {
        //대쉬 중일땐 대쉬 못하도록 막기
        if (isDash) return; 

        StartCoroutine(DashCo());
    }
    IEnumerator DashCo()
    {
        canDash = false;
        isDash = true;

        //공중에서 대쉬 쓸 땐 중력을 0으로
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(checkDir * dashPower, 0.0f);
        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originGravity;
        isDash = false;

        //대쉬 쿨타임
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void CheckDir()
    {
        if (InputManager.Movement.x > 0)
        {
            checkDir = 1f;
        }
        else if (InputManager.Movement.x < 0)
        {
            checkDir = -1f;
        }

        transform.localScale = new Vector3(originScale.x * checkDir, originScale.y, originScale.z);
    }
    private void Jump()
    {
        if (jumpcount >= maxJumpCount) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        jumpcount++;

        isGround = false;
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(InputManager.Movement.x * moveSpeed, rb.linearVelocity.y);
    }
    private void CheckGround()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);
        if (isGround) jumpcount = 0;
    }
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGround ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

}
