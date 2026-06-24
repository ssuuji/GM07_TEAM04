using UnityEngine;

public class PoisonBall : MonoBehaviour
{

    [SerializeField] private float throwingPowerMin = 2.0f;
    [SerializeField] private float throwingPowerMax = 5.0f;
    [SerializeField] private Boss boss;
    [SerializeField] private GameObject floorPrefab;

    private float throwingPower;
    private Rigidbody2D rb;

    public void Init(Boss boss)
    {
        this.boss = boss;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        throwingPower = Random.Range(throwingPowerMin, throwingPowerMax);
        if (boss.Direction)
        {
            rb.AddForce(Vector2.right * throwingPower, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * throwingPower, ForceMode2D.Impulse);
        }
        
        rb.AddForce(Vector2.up * throwingPower, ForceMode2D.Impulse);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(floorPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
