using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpPower = 3.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        CheckGround();

        if (InputManager.IsJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Jump()
    {
        if (!isGround) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(InputManager.Movement.x * moveSpeed, rb.linearVelocity.y);
    }
    private void CheckGround()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

}
