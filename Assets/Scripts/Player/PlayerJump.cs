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
    [SerializeField] private Animator spumAnimator;

    [Header("사운드 설정")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    [Header("점프 이펙트")]
    [SerializeField] private GameObject jumpEffectPrefab;
    [SerializeField] private Transform jumpEffectPoint;

    [Header("체공 설정")]
    [SerializeField] private float floatGravity = 0.5f;
    [SerializeField] private float normalGravity = 3f;
    [SerializeField] private float minFallSpeed = -2f;

    private Rigidbody2D rb;
    private bool isGround; //바닥 체크
    private int jumpCount; //현재 점프 횟수

    public bool IsGround => isGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spumAnimator == null)
        {
            spumAnimator = GetComponentInChildren<Animator>();
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        //최대점프횟수 까지만 점프 가능
        if (jumpCount >= maxJumpCount) return;
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); //x속도는 유지하고 y속도만 점프힘으로 변경
        jumpCount++; //점프 횟수 증가

        //사운드
        audioSource.PlayOneShot(jumpSound);

        //점프애니메이션이 없어서 Idle 이미지(?)로 대체
        if (spumAnimator != null)
        {
            //이펙트
            Instantiate(jumpEffectPrefab, jumpEffectPoint.position, Quaternion.identity, jumpEffectPoint);

            spumAnimator.Play("IDLE", 0, 0f); //Idle 상태를 BaseLayer(0) 에서 재생시간(0f) 부터 실행
            spumAnimator.Update(0f);          //위에 Play()로 바꾼 상태를 프레임에 바로 반영 
            spumAnimator.enabled = false;     //잠시 애니메이션 비활성화
        }
    }

    public void CheckGround()
    {
        bool wasGround = isGround; //이전 프레임의 바닥체크 저장

        // 바닥 체크
        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        // 공중상태 + 아래로 내려가는 중 + 바닥에 착지한 순간 : 점프 횟수 초기화
        if (!wasGround && rb.linearVelocity.y <= 0f && isGround)
        {
            jumpCount = 0;

            // Animator 다시 활성화
            if (spumAnimator != null && !spumAnimator.enabled)
            {
                spumAnimator.enabled = true;
            }
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
}