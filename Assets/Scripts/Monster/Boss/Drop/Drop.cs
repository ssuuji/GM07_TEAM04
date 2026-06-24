using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float dropSpeed = 5.0f;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private PlayerHealth player;
    private Rigidbody2D rb;
    private float currentLifeTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player= GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
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

    private void Aging()
    {
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime <= 0)
        {
            Die();
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(0, -dropSpeed);
        dropSpeed += 0.2f;
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }
}
