using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격범위")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(1.8f, 1.5f);
    [SerializeField] private Transform areaAttackPoint;
    [SerializeField] private Vector2 areaAttackSize = new Vector2(2.5f, 1.5f);

    [Header("공격 이펙트")]
    [SerializeField] private GameObject attackEffectPrefab;
    [SerializeField] private GameObject areaAttackEffectPrefab;
    [SerializeField] private Transform slashEffectPoint;

    [Header("버프 이펙트")]
    [SerializeField] private GameObject buffEffectPrefab;
    [SerializeField] private Transform buffEffectPoint;

    [Header("기본공격 설정")]
    [SerializeField] private float attackCooldown = 0.5f;    //기본공격 쿨타임
    private bool canAttack = true;

    [Header("범위공격 설정")]
    [SerializeField] private int areaAttackMpCost = 10;       //범위공격 소모 MP
    [SerializeField] private float areaAttackCooldown = 3.0f; //범위공격 쿨타임
    private bool canAreaAttack = true;                        //범위공격 사용가능 여부

    [Header("공격버프 설정")]
    [SerializeField] private int buffMpCost = 15;         //공격버프 소모 MP
    [SerializeField] private int buffAmount = 10;         //공격버프 증가량(공격력 +10)
    [SerializeField] private float buffDuration = 10.0f;  //공격버프 지속시간
    [SerializeField] private float buffCooldown = 30.0f;  //공격버프 쿨타임
    private bool isBuff;                                  //현재 공격버프 적용중인지 확인여부
    private bool canBuff = true;                          //공격버프 사용가능 여부

    [Header("무적스킬 설정")]
    [SerializeField] private int invinMpCost = 30;        //무적기 소모 MP
    [SerializeField] private float invinDuration = 3.0f;  //무적기 지속시간
    [SerializeField] private float invinCooldown = 30.0f; //무적기 쿨타임
    private bool isInvin;                                 //현재 무적상태인지 확인여부
    private bool canInvin = true;                         //무적기 사용가능 여부

    [Header("몬스터 레이어")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("애니메이션 설정")]
    [SerializeField] private Animator spumAnimator;

    private PlayerStatus playerStatus;
    private PlayerMovement playerMove;
    private PlayerHealth playerHealth;
    private PlayerSkill playerSkill;

    //스킬 쿨타임/지속시간 UI 표시용 (남은시간)
    private float areaAttackCooldownRemain;
    private float buffCooldownRemain;
    private float buffDurationRemain;
    private float invinCooldownRemain;
    public float AreaAttackCooldownRemain => areaAttackCooldownRemain;
    public float BuffCooldownRemain => buffCooldownRemain;
    public float BuffDurationRemain => buffDurationRemain;
    public float InvinCooldownRemain => invinCooldownRemain;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerMove = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSkill = GetComponent<PlayerSkill>();
        spumAnimator = GetComponentInChildren<Animator>();
    }

    #region 기본공격
    //기본공격 : 공격범위 안에서 플레이어 가장 앞에 있는 몬스터 1마리 공격
    public void Attack()
    {
        if (!canAttack) return;

        //쿨타임 : 너무 빠르게 재사용 할 수 없도록..
        StartCoroutine(AttackCooldownCo());

        //애니메이션 : 몬스터가 없어도 공격모션이 나오게끔
        if (spumAnimator != null)
        {
            spumAnimator.SetTrigger("2_Attack");
        }

        //이펙트
        Instantiate(attackEffectPrefab, slashEffectPoint.position, Quaternion.identity);

        // 공격범위 안 enemy레이어를 가진 몬스터 콜라이더 리스트
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayer);
        if (hits.Length == 0) return;

        //범위 안 몬스터 중 공격할 1마리 추출
        Collider2D target = GetTarget(hits);
        if (target == null) return;

        //target이 데미지를 받을 수 있는 대상이라면
        IDamageable damageable = target.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            //데미지 적용
            damageable.TakeDamage(playerStatus.CurrentAttack);
        }

        Debug.Log($"{target.name} 공격");
    }

    private Collider2D GetTarget(Collider2D[] hits)
    {
        Collider2D targetMonster = null;

        //플레이어가 바라보고 있는 방향 
        float dir = playerMove.CheckDirValue; //오른쪽 : 1 , 왼쪽: -1

        //기준값(플레이어 <-> 몬스터 가장 가까운 거리)
        float front = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            //플레이어 기준 몬스터와의 거리 계산(distance)
            //오른쪽을 보고있다면 :  몬스터 x - 플레이어 x
            //왼쪽을 보고있다면   : (몬스터 x - 플레이어 x) * -1
            float distance = (hit.transform.position.x - transform.position.x) * dir;

            //플레이어 앞쪽에 있으면서 현재까지 찾은 몬스터보다 더 가까운 몬스터라면
            if (distance > 0 && distance < front)
            {
                //기준값(front) 갱신
                front = distance;
                //공격대상 몬스터 갱신
                targetMonster = hit;
            }
        }

        return targetMonster;
    }

    IEnumerator AttackCooldownCo()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    #endregion

    #region 범위공격
    //범위공격 : 공격 범위 안 몬스터들 모두 공격
    public void AreaAttack()
    {
        if (!playerSkill.AreaAttackUnlocked) return;       //스킬해금이 되지 않았으면 X : (2레벨부터 사용가능)
        if (!canAreaAttack) return;                        //쿨타임 중이라면 X
        if (!playerStatus.UseMp(areaAttackMpCost)) return; //MP가 부족하면 X

        //애니메이션 
        if (spumAnimator != null)
        {
            spumAnimator.SetTrigger("2_Attack");
        }

        //이펙트
        Instantiate(areaAttackEffectPrefab, slashEffectPoint.position, Quaternion.identity);

        //범위공격스킬 쿨타임 시작
        StartCoroutine(AreaAttackCooldownCo());

        // 공격범위 안 enemy레이어를 가진 몬스터 콜라이더 리스트
        Collider2D[] hits = Physics2D.OverlapBoxAll(areaAttackPoint.position, areaAttackSize, 0f, enemyLayer);
        if (hits.Length == 0) return;

        //해당 범위의 모든 몬스터들 공격
        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(playerStatus.CurrentAttack);
            }

            Debug.Log($"{hit.name} 공격");
        }
    }
    IEnumerator AreaAttackCooldownCo()
    {
        canAreaAttack = false;
        
        //UI에 표시할 쿨타임 남은시간 설정
        areaAttackCooldownRemain = areaAttackCooldown;
        
        //남은시간이 0이 될 때 까지 매 프레임 감소시키기
        while (areaAttackCooldownRemain > 0)
        {
            areaAttackCooldownRemain -= Time.deltaTime;
            yield return null;
        }
        
        //쿨타임 종료
        areaAttackCooldownRemain = 0;
        canAreaAttack = true;
    }
    #endregion

    #region 공격버프
    //공격버프 :  10초간 공격력을 +10 해준다. 쿨타임은 30초
    public void Buff()
    {
        if (!playerSkill.BuffUnlocked) return;       //스킬해금 되지않았으면 X : (3레벨부터 사용가능)
        if (!canBuff) return;                        //쿨타임 중이면 X
        if (isBuff) return;                          //이미 버프가 적용중이면 X
        if (!playerStatus.UseMp(buffMpCost)) return; //MP가 부족하면 X

        //애니메이션
        if (spumAnimator != null)
        {
            spumAnimator.SetTrigger("5_Buff");
        }

        //이펙트
        Instantiate(buffEffectPrefab, buffEffectPoint.position, Quaternion.identity);

        //버프 지속시간
        StartCoroutine(BuffCo());
        //공격버프 스킬 쿨타임
        StartCoroutine(BuffCooldownCo());
    }
    IEnumerator BuffCo()
    {
        isBuff = true;

        //버프(공격력 증가) 적용
        playerStatus.AddAttack(buffAmount);
        Debug.Log("공격버프 시작");

        //UI에 표시할 버프 지속시간
        buffDurationRemain = buffDuration;

        //버프 지속시간이 끝날 때 까지 매 프레임 감소시키기
        while (buffDurationRemain > 0)
        {
            buffDurationRemain -= Time.deltaTime;
            yield return null;
        }

        //버프 종료
        buffDurationRemain = 0;

        //플레이어 공격력 복구
        playerStatus.RemoveAttack(buffAmount);
        isBuff = false;

        Debug.Log("공격버프 종료");
    }
    IEnumerator BuffCooldownCo()
    {
        canBuff = false;

        //UI에 표시할 쿨타임 남은시간
        buffCooldownRemain = buffCooldown;

        //쿨타임이 끝날 때 까지 매 프레임 감소시키기
        while (buffCooldownRemain > 0)
        {
            buffCooldownRemain -= Time.deltaTime;
            yield return null;
        }

        //쿨타임 종료
        buffCooldownRemain = 0;
        canBuff = true;
        Debug.Log("공격버프 사용가능");
    }
    #endregion

    #region 무적기
    // 무적기 : 플레이어는 3초동안 데미지를 받지 않는다. 쿨타임은 30초
    public void Invin()
    {
        if (!playerSkill.InvinUnlocked) return;       //스킬이 해금되지 않았으면 X : 4레벨부터 사용가능
        if (!canInvin) return;                        //쿨타임 중이면 X
        if (isInvin) return;                          //이미 무적상태면 X
        if (!playerStatus.UseMp(invinMpCost)) return; //MP가 부족하면 X

        //애니메이션
        if (spumAnimator != null)
        {
            spumAnimator.SetTrigger("5_Buff");
        }

        //이펙트
        Instantiate(buffEffectPrefab, buffEffectPoint.position, Quaternion.identity);

        //무적기 지속시간
        StartCoroutine(InvinCo());
        //무적기 쿨타임
        StartCoroutine(InvinCooldownCo());
    }
    IEnumerator InvinCo()
    {
        isInvin = true;

        //SetInvin 으로 무적상태 적용
        playerHealth.SetInvin(true);
        Debug.Log("무적기 시작");

        //무적시간동안 대기
        yield return new WaitForSeconds(invinDuration);

        //무적상태 해제
        playerHealth.SetInvin(false);
        Debug.Log("무적기 종료");

        isInvin = false;
    }
    IEnumerator InvinCooldownCo()
    {
        canInvin = false;

        //UI에 표시할 쿨타임 남은 시간
        invinCooldownRemain = invinCooldown;

        //쿨타임이 끝날 때 까지 매 프레임 감소시키기
        while (invinCooldownRemain > 0)
        {
            invinCooldownRemain -= Time.deltaTime;
            yield return null;
        }

        //쿨타임 종료
        invinCooldownRemain = 0;
        canInvin = true;
        Debug.Log("무적기 사용가능");
    }
    #endregion

    private void OnDrawGizmos()
    {
        //공격 범위 확인 Gizmos
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(attackPoint.position, attackSize);
        }
        if (areaAttackPoint != null)
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireCube(areaAttackPoint.position, areaAttackSize);
        }
    }
}
