using DamageNumbersPro;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("애니메이션 설정")]
    private Animator playerAnim;  
    [SerializeField] private CanvasGroup fadeImage;  
    [SerializeField] private float dieTime = 1.0f; // Die애니메이션 시간
    [SerializeField] private float fadeTime = 1.0f;  // 화면 전환 시간

    [Header("데미지 숫자")]
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private Transform damageNumberPoint;

    [Header("넉백 설정")]
    [SerializeField] private float knockBackPower = 5.0f;
    [SerializeField] private float knockBackTime = 0.15f;
    
    private PlayerStatus playerStatus;
    private PlayerMovement playerMovement; 
    private Rigidbody2D rb;
    private bool isKnockBack;            //넉백 중인지 확인여부
    private bool isDead;                 //플레이어 사망여부
    private bool isInvin;                //무적상태 확인여부
    private Coroutine currentCo;
    public bool IsKnockBack => isKnockBack;
    public bool IsDead => isDead;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        if (playerAnim == null)
        {
            playerAnim = GetComponentInChildren<Animator>();
        }
    }

    //데미지 받을 때
    public void TakeDamage(int damage)
    {
        if (isDead) return;  //이미 사망상태면 X
        if (isInvin) return; //무적상태면 X

        //damage만큼 HP 감소
        playerStatus.TakeDamage(damage);

        //받은 데미지 숫자 표시
        damageNumberPrefab.Spawn(damageNumberPoint.position, damage); //DamageNumbersPro 에셋에서 Spawn을 지원함

        //HP가 0이하라면 사망처리(Die)
        if (playerStatus.CurrentHp <= 0)
        {
            Die();
            return;
        }

        //애니메이션
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Hurt");
        }

        Knockback();
        DamageColor();
    }

    private void Knockback()
    {
        StartCoroutine(KnockbackCo());
    }

    IEnumerator KnockbackCo()
    {
        isKnockBack = true;

        //플레이어가 바라보는 방향의 반대방향으로 
        float dir = -playerMovement.CheckDirValue;

        //넉백 적용
        rb.linearVelocity = new Vector2(dir * knockBackPower, rb.linearVelocity.y);

        yield return new WaitForSeconds(knockBackTime);

        isKnockBack = false;
    }

    private void DamageColor()
    {
        if (currentCo != null)
        {
            StopCoroutine(currentCo);
        }
        currentCo = StartCoroutine(ColorCo());
    }

    IEnumerator ColorCo()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        Color originColor = renderer.material.color;

        renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        renderer.material.color = originColor;

        currentCo = null;
    }

    //플레이어 사망처리
    private void Die()
    {
        isDead = true;

        // 애니메이션
        if (playerAnim != null)
        {
            playerAnim.SetBool("IsDead", true);
        }

        //사망후 멈추기
        rb.linearVelocity = Vector2.zero;

        Debug.Log("플레이어 사망");

        StartCoroutine(DieCo());
    }

    IEnumerator DieCo()
    {
        //Die 애니메이션 재생시간
        yield return new WaitForSeconds(dieTime);

        //화면 페이드 아웃
        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            fadeImage.alpha = timer / fadeTime;

            yield return null;
        }

        fadeImage.alpha = 1f;

        // 게임 오버 씬 이동
        GameSceneManager.Instance.LoadScene(SceneType.GameOver);
    }


    //무적 상태 변경
    public void SetInvin(bool value)
    {
        isInvin = value;
    }
}
