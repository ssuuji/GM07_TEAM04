using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float dropSpeed = 5.0f;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private PlayerHealth player;
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private GameObject GroundFxPrefab;


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
        SFXManager.Instance.PlayDrop();
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
            SFXManager.Instance.PlayExplode();
            Instantiate(GroundFxPrefab, transform.position, Quaternion.identity);
            Die();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            SFXManager.Instance.PlayDropHit();
            Instantiate(hitFxPrefab, transform.position, Quaternion.identity);
            player.TakeDamage(damage);
        }
    }
}
