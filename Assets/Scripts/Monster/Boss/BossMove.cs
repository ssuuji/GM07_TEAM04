using UnityEngine;

public class BossMove : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Boss boss;
    private Rigidbody2D rb;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(boss.Direction)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }
    }
}
