using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("대쉬 설정")]
    [SerializeField] private float dashPower = 10.0f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;

    private Rigidbody2D rb;
    private float originGravity;
    private bool isDash;
    private bool canDash = true;

    public bool IsDash => isDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale;
    }

    public void Dash(float dir)
    {
        if (isDash) return;
        if (!canDash) return;

        StartCoroutine(DashCo(dir));
    }

    private IEnumerator DashCo(float dir)
    {
        canDash = false;
        isDash = true;

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(dir * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originGravity;
        isDash = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}