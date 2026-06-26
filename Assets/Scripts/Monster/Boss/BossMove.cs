using UnityEngine;

public class BossMove : MonoBehaviour
{
    [SerializeField] private float reach = 10.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Boss boss;
    [SerializeField] private GameObject player;

    [SerializeField] private BossAnimation bossAnimation;

    private Rigidbody2D rb;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        rb = GetComponent<Rigidbody2D>();
        bossAnimation = GetComponent<BossAnimation>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (rb.linearVelocity == Vector2.zero) bossAnimation.EndMove();
        VerticalMove();
        if (Reach())
        {
            //HorizontalMove(1);
        }
        else
        {
            HorizontalMove(-1);
        }
    }

    private void HorizontalMove(int reverse)
    {
        if (boss.Direction)
        {
            rb.linearVelocity = new Vector2(-speed * reverse, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(speed * reverse, rb.linearVelocity.y);
        }
    }

    private void VerticalMove()
    {
        if (player.transform.position.y > transform.position.y)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);
        }

        if (player.transform.position.y < transform.position.y)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -speed);
        }
    }

    private bool Reach()
    {
        if(Mathf.Abs(player.transform.position.x - transform.position.x) < reach)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
