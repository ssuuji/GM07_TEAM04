using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerHealth player;

    [SerializeField] private int damage = 10;
    [SerializeField] private float bulletSpeed = 2.0f;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private GameObject hitFxPrefab;


    private Rigidbody2D rb;
    private bool direction;
    private float currentLifeTime;
    private Vector2 dir;

    public void Init(Boss boss)
    {
        this.boss = boss;
        direction = boss.Direction;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        SFXManager.Instance.PlayBullet();
    }

    private void OnEnable()
    {
        currentLifeTime = lifeTime;
        dir = (player.transform.position - transform.position).normalized;
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
        rb.linearVelocity = dir * bulletSpeed;
        bulletSpeed += 0.1f;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            SFXManager.Instance.PlayBulletHit();
            Instantiate(hitFxPrefab, transform.position, Quaternion.identity);
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Ground"))
        {
            SFXManager.Instance.PlayBulletHit();
            Instantiate(hitFxPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
