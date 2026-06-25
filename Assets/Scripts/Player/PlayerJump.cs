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

        //x속도는 유지하고 y속도만 점프힘으로 변경
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        jumpCount++;      //점프 횟수 증가
        isGround = false; //점프 했으니까 아직 공중상태 유지

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
        // 바닥 체크
        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        // 바닥에 닿아 있으면 점프 횟수 초기화
        if (isGround)
        {
            jumpCount = 0;

            // Animator 다시 활성화
            if (spumAnimator != null && !spumAnimator.enabled)
            {
                spumAnimator.enabled = true;
            }
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