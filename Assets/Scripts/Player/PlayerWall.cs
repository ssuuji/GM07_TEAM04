using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    [Header("벽 체크")]
    [SerializeField] private Transform wallCheck;                         //벽 체크
    [SerializeField] private Vector2 checkSize = new Vector2(0.3f, 1.7f); //벽 체크범위
    [SerializeField] private LayerMask wallLayer;                         //벽 체크레이어

    [Header("슬라이드")]
    [SerializeField] private float wallSlideSpeed = 0.5f;                 //벽 슬라이드 속도
    private bool isWall;                                                  //벽에 닿아있는지 확인여부
    private bool isWallSliding;                                           //슬라이드중인지 확인여부


    [Header("벽 점프")]
    [SerializeField] private Vector2 wallJumpPower = new Vector2(7f, 10f);//벽 점프 힘(반대방향 힘, 위쪽방향 힘)
    [SerializeField] private float wallJumpTime = 0.2f;                   //벽 점프 후 입력제한 시간
    private bool isWallJump;                                              //현재 벽 점프중인지 확인
    private float wallJumpTimer;                                          //벽 점프중 상태유지 시간

    private Animator playerAnim;
    private Rigidbody2D rb;
    private PlayerJump playerJump;
    private PlayerMovement playerMovement;

    public bool IsWall => isWall;
    public bool IsWallJump => isWallJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovement>();
        if (playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
    }

    //벽에 닿아있는지 체크
    public void CheckWall()
    {
        if (wallCheck == null) return;
        isWall = Physics2D.OverlapBox(wallCheck.position, checkSize, 0f, wallLayer);
    }

    //벽 슬라이드
    public void WallSlide()
    {
        isWallSliding = false;

        if (!isWall || playerJump.IsGround)
        {
            UpdateWallSlideAnimation();
            return;
        }

        //천천히 내려오기
        isWallSliding = true;
        rb.linearVelocity = new Vector2(0f, -wallSlideSpeed); 

        UpdateWallSlideAnimation();
    }

    //슬라이딩 애니메이션
    private void UpdateWallSlideAnimation()
    {
        if (playerAnim != null)
        {
            playerAnim.SetBool("IsWallSliding", isWallSliding);
        }
    }

    public void WallJump()
    {
        if (!isWall) return;             //벽에 닿아있지 않으면 X
        if (playerJump.IsGround) return; //바닥에 있으면 X

        isWallJump = true;                                                           //벽점프 상태 시작
        wallJumpTimer = wallJumpTime;                                                //벽점프 상태 유지시간 설정
        float jumpDir = -playerMovement.CheckDirValue;                               //현재 바라보는 방향 반대로 점프하기
        rb.linearVelocity = new Vector2(jumpDir * wallJumpPower.x, wallJumpPower.y); //x축은 벽 반대방향 y축은 위로 점프파워적용

        //애니메이션
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Jump");
        }
    }

    //벽점프 이후 이동입력 막기
    public void UpdateWallJump()
    {
        //벽점프 상태가 아니라면 X
        if (!isWallJump) return; 

        //벽점프 상태시간 감소
        wallJumpTimer -= Time.deltaTime;

        //벽점프 상태 종료
        if (wallJumpTimer <= 0)
        {
            isWallJump = false;
        }
    }

    private void OnDrawGizmos()
    {
        //벽 체크 Gizmos
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(wallCheck.position, checkSize);
    }
}
