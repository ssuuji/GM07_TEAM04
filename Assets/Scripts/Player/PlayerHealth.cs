using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStatus playerStatus;
    private bool isDead;       //플레이어 사망여부
    private bool isInvin;      //무적상태 확인여부

    private PlayerMovement playerMovement; 
    private Rigidbody2D rb;
    private float knockBackPower = 5.0f; //넉백 파워
    private float knockBackTime = 0.15f; //넉백 유지시간
    private bool isKnockBack;            //넉백 중인지 확인여부
    private Coroutine currentCo;         //피격 색상 코루틴

    public bool IsKnockBack => isKnockBack;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    //데미지 받을 때
    public void TakeDamage(int damage)
    {
        if (isDead) return;  //이미 사망상태면 X
        if (isInvin) return; //무적상태면 X

        //damage만큼 HP 감소
        playerStatus.TakeDamage(damage);

        //HP가 0이하라면 사망처리(Die)
        if (playerStatus.CurrentHp <= 0)
        {
            Die();
        }

        //피격시 넉백 적용 및 색상처리(추후 색상 -> 애니메이션으로 변경예정)
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

        renderer.material.color = Color.yellow;

        yield return new WaitForSeconds(0.15f);

        renderer.material.color = originColor;

        currentCo = null;
    }


    //플레이어 사망처리
    private void Die()
    {
        isDead = true;
        Debug.Log("플레이어 사망");

        GameSceneManager.Instance.LoadScene(SceneType.GameOver);
    }

    //무적 상태 변경
    public void SetInvin(bool value)
    {
        isInvin = value;
    }
}
