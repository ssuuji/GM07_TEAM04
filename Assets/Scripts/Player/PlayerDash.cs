using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("대쉬 설정")]
    [SerializeField] private float dashPower = 20.0f;   //대쉬 속도
    [SerializeField] private float dashTime = 0.2f;     //대쉬 지속시간
    [SerializeField] private float dashCooldown = 1.0f; //대쉬 쿨타임

    private Rigidbody2D rb;
    private float originGravity;  //기존 중력값 저장
    private bool isDash;          //현재 대쉬 중인지 확인여부
    private bool canDash = true;  //대쉬 사용가능 여부

    public bool IsDash => isDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale; //대쉬 종료 후 복구할 기존 중력값 저장
    }

    public void Dash(float dir)
    {
        if (isDash) return;   //이미 대쉬 중이면 X
        if (!canDash) return; //쿨타임 중이면 X

        //대쉬
        StartCoroutine(DashCo(dir));
    }

    private IEnumerator DashCo(float dir)
    {
        canDash = false; //대쉬 사용 불가능(대쉬 쿨타임 시작)
        isDash = true;   //대쉬상태

        //대쉬 중일 땐 중력 0 으로설정해서 밑으로 내려가지 않도록
        rb.gravityScale = 0f;
        // 플레이어가 바라보는 방향(dir)로 대쉬
        rb.linearVelocity = new Vector2(dir * dashPower, 0f); 

        //대쉬 지속시간만큼 대기
        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originGravity; //기존 중력값 복구
        isDash = false;                  //대쉬상태 종료

        //쿨타임 대기
        yield return new WaitForSeconds(dashCooldown);

        //대쉬 사용가능
        canDash = true;
    }
}