using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("점프 설정")]
    [SerializeField] private float jumpPower = 12.0f; 
    [SerializeField] private int maxJumpCount = 2;   //최대 점프 횟수

    [Header("바닥 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.08f);
    [SerializeField] private LayerMask groundLayer;

    [Header("애니메이션 설정")]
    [SerializeField] private Animator playerAnim;

    [Header("사운드 설정")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    [Header("점프 이펙트")]
    [SerializeField] private GameObject jumpEffectPrefab;
    [SerializeField] private Transform jumpEffectPoint;

    [Header("체공 설정")]
    [SerializeField] private float floatGravity = 0.5f;
    [SerializeField] private float normalGravity = 3f;
    [SerializeField] private float minFallSpeed = -2f;

    [Header("코요테 타임")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    private Rigidbody2D rb;
    private bool isGround; //바닥 체크
    [SerializeField]private int jumpCount; //현재 점프 횟수

    public bool IsGround => isGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        //Jump / Fall 상태확인
        if (playerAnim != null)
        {
            playerAnim.SetBool("IsGrounded", isGround);
            playerAnim.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        }
    }

    public void Jump() // 코요테 타임 적용을 위해 수정함
    {
        bool canCoyoteJump = coyoteTimeCounter > 0f && jumpCount == 0;

        if (canCoyoteJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCount = 1;
            coyoteTimeCounter = 0f;
            PlayJumpEffect();
        }
        else if (jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCount++;
            coyoteTimeCounter = 0f;
            PlayJumpEffect();
        }
    }

    private void PlayJumpEffect()
    {
        //오디오
        if (audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
        //이펙트
        if (jumpEffectPrefab != null && jumpEffectPoint != null)
        {
            Instantiate(jumpEffectPrefab, jumpEffectPoint.position, Quaternion.identity, jumpEffectPoint);
        }
        //애니메이션
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Jump");
        }
    }

    public void CheckGround()
    {
        //바닥감지
        bool checkGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);
        //위로올라가는중
        bool isUp = rb.linearVelocity.y > 0.05f;

        //바닥에 닿아있고 위로 올라가는중이 아니라면 : Ground True
        isGround = checkGround && !isUp;

        if (isGround)
        {
            jumpCount = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter = Mathf.Max(coyoteTimeCounter - Time.deltaTime, 0f);
        }
    }

    //체공
    public void Hovering()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.spaceKey.isPressed && !isGround && rb.linearVelocity.y < 0)
        {
            rb.gravityScale = floatGravity;

            if (rb.linearVelocity.y < minFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, minFallSpeed);
            }
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    private void OnDrawGizmos()
    {
        //바닥 범위 확인 Gizmo
        if (groundCheck != null)
        {
            Gizmos.color = isGround ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheck.position, checkSize);
        }
    }

    private void OnGUI() // 코요테 타임 확인용 gui 
    {
        //GUI.color = coyoteTimeCounter > 0f ? Color.green : Color.red;
        //GUI.Label(new Rect(10, 10, 200, 30), $"CoyoteTime: {coyoteTimeCounter:F2}");
    }
}