using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("애니메이션 설정")]
    [SerializeField] private Animator spumAnimator;

    [Header("이동 사운드")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private float moveSoundInterval = 0.4f; //발소리 재생간격
    private Coroutine SoundCo;

    private PlayerJump playerJump;
    private PlayerDash playerDash;

    private Rigidbody2D rb;
    private Vector3 originScale;  // 플레이어 기존 크기 저장
    private float checkDir = 1f;  // 플레이어가 바라보는 방향 저장

    public float CheckDirValue => checkDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();
    }

    //좌우 이동
    public void Move()
    {
        //입력 값에 따라 X 속도 변경하고 y 속도는 그대로 유지
        rb.linearVelocity = new Vector2(InputManager.Movement.x * moveSpeed, rb.linearVelocity.y);

        //이동중인지 확인
        bool isMove = Mathf.Abs(InputManager.Movement.x) > 0.01f;

        //점프, 대쉬 중이 아니라면 발소리 O
        bool canPlaySound = isMove && playerJump.IsGround && !playerDash.IsDash;
        //사운드 코루틴
        if (canPlaySound)
        {
            if (SoundCo == null)
            {
                SoundCo = StartCoroutine(MoveSoundCo());
            }
        }
        else
        {
            if (SoundCo != null)
            {
                StopCoroutine(SoundCo);
                SoundCo = null;
            }
        }

        //애니메이션
        if (spumAnimator != null)
        {
            //true : MOVE / flase : Idle
            spumAnimator.SetBool("1_Move", isMove);
        }
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

    //발소리 사운드
    IEnumerator MoveSoundCo()
    {
        //움직이는 동안 계속 사운드 반복
        while (true)
        {
            audioSource.PlayOneShot(moveSound);

            yield return new WaitForSeconds(moveSoundInterval);
        }
    }
}