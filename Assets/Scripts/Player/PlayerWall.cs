using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    [Header("벽 체크")]
    [SerializeField] private Transform wallCheck;                         //벽 체크
    [SerializeField] private Vector2 checkSize = new Vector2(0.1f, 0.8f); //벽 체크범위
    [SerializeField] private LayerMask wallLayer;                         //벽 체크레이어

    [Header("슬라이드")]
    [SerializeField] private float wallSlideSpeed = 0.5f;                 //벽 슬라이드 속도
    private bool isWall;                                                  //벽에 닿아있는지 확인여부

    [Header("벽 점프")]
    [SerializeField] private Vector2 wallJumpPower = new Vector2(7f, 10f);//벽 점프 힘(반대방향 힘, 위쪽방향 힘)
    [SerializeField] private float wallJumpTime = 0.2f;                   //벽 점프 후 입력제한 시간
    private bool isWallJump;                                              //현재 벽 점프중인지 확인
    private float wallJumpTimer;                                          //벽 점프중 상태유지 시간

    [Header("애니메이션 설정")]
    [SerializeField] private Animator spumAnimator;

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
        
        //애니메이션
        if (spumAnimator == null)
        {
            spumAnimator = GetComponentInChildren<Animator>();
        }
    }

    //벽에 닿아있는지 체크
    public void CheckWall()
    {
        isWall = Physics2D.OverlapBox(wallCheck.position, checkSize, 0f, wallLayer);
    }

    //벽 슬라이드
    public void WallSlide()
    {
        if (!isWall) return;             //벽에 닿아있지 않으면 실행 X
        if (playerJump.IsGround) return; //플레이어가 바닥에 있으면 실행 X

        //현재 좌우 입력값
        float moveX = InputManager.Movement.x;

        //플레이어가 바라보는 방향
        float dir = playerMovement.CheckDirValue;

        //벽 방향으로 방향키를 누르고 있는지 확인
        bool isInputWall = moveX != 0 && Mathf.Sign(moveX) == dir;

        //애니메이션
        if (isInputWall)
        {
            //벽타기 애니메이션이 없어서 .. Idle 상태로
            if (spumAnimator != null)
            {
                spumAnimator.SetBool("7_Jump", false);
                spumAnimator.SetBool("1_Move", false);
            }
        }

        //아래로 떨어지고 있으면서 벽쪽으로 방향키를 누르고 있을 때 천천히 내려오기
        if (rb.linearVelocity.y < 0 && isInputWall)
        {
            rb.linearVelocity = new Vector2(0f, -wallSlideSpeed);
        }
    }

    public void WallJump()
    {
        if (!isWall) return;             //벽에 닿아있지 않으면 X
        if (playerJump.IsGround) return; //바닥에 있으면 X

        //벽점프 상태 시작
        isWallJump = true;
        //벽점프 상태 유지시간 설정
        wallJumpTimer = wallJumpTime;

        //현재 바라보는 방향 반대로 점프하기
        float jumpDir = -playerMovement.CheckDirValue;

        //x축은 벽 반대방향 y축은 위로 점프파워적용
        rb.linearVelocity = new Vector2(jumpDir * wallJumpPower.x, wallJumpPower.y);
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
