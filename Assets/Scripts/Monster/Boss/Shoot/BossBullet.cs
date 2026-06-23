using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerHealth player;

    [SerializeField] private int damage = 10;
    [SerializeField] private float bulletSpeed = 2.0f;
    [SerializeField] private float lifeTime = 5.0f;

    private Rigidbody2D rb;
    private bool direction;
    private float currentLifeTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
    }

    private void OnEnable()
    {
        direction = boss.Direction;
        currentLifeTime = lifeTime;
    }

    private void Update()
    {
        Aging();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (direction)
        {
            rb.linearVelocity = new Vector2 (bulletSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2 (-bulletSpeed, 0);
        }
        bulletSpeed += 0.2f;
    }

    private void Aging()
    {
        currentLifeTime -= Time.deltaTime;

        if(currentLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
