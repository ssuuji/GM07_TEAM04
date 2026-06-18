using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("점프 설정")]
    [SerializeField] private float jumpPower = 8.0f;
    [SerializeField] private int maxJumpCount = 2;

    [Header("바닥 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.08f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGround;
    private int jumpCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        if (jumpCount >= maxJumpCount) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        jumpCount++;
        isGround = false;
    }

    public void CheckGround()
    {
        bool wasGround = isGround;

        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        if (!wasGround && isGround && rb.linearVelocity.y <= 0f)
        {
            jumpCount = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGround ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }
    }
}