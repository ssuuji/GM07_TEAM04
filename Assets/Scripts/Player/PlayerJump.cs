using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("점프 설정")]
    [SerializeField] private float jumpPower = 8.0f; 
    [SerializeField] private int maxJumpCount = 2;   //최대 점프 횟수

    [Header("바닥 체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.08f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGround; //바닥 체크
    private int jumpCount; //현재 점프 횟수

    public bool IsGround => isGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        //최대점프횟수 까지만 점프 가능
        if (jumpCount >= maxJumpCount) return;

        //x속도는 유지하고 y속도만 점프힘으로 변경
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        jumpCount++;      //점프 횟수 증가
        isGround = false; //점프 했으니까 아직 공중상태 유지
    }

    public void CheckGround()
    {
        //이전 바닥체크 상태 저장
        bool wasGround = isGround;

        //바닥체크
        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        // 이전에는 공중상태이고 현재는 바닥상태 이며 아래로 내려오고 있는 중이라면 점프횟수 초기화
        if (!wasGround && isGround && rb.linearVelocity.y <= 0f)
        {
            jumpCount = 0;
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