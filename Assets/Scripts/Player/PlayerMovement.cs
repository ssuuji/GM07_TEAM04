using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector3 originScale;  // 플레이어 기존 크기 저장
    private float checkDir = 1f;  // 플레이어가 바라보는 방향 저장

    public float CheckDirValue => checkDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
    }

    //좌우 이동
    public void Move()
    {
        //입력 값에 따라 X 속도 변경하고 y 속도는 그대로 유지
        rb.linearVelocity = new Vector2(InputManager.Movement.x * moveSpeed, rb.linearVelocity.y);
    }

    //플레이어 방향체크
    public void CheckDir()
    {
        //오른쪽
        if (InputManager.Movement.x > 0)
        {
            checkDir = 1f;
        }
        //왼쪽
        else if (InputManager.Movement.x < 0)
        {
            checkDir = -1f;
        }

        //현재 방향에 따라 플레이어 좌우 반전
        transform.localScale = new Vector3(Mathf.Abs(originScale.x) * checkDir, originScale.y, originScale.z);
    }
}