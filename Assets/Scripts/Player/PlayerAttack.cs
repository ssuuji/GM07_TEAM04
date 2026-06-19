using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격범위")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(2.5f, 1.5f);

    [Header("범위공격 설정")]
    [SerializeField] private int areaAttackMpCost = 10;       //범위공격 소모마나량
    [SerializeField] private float areaAttackCooldown = 3.0f; //범위공격 쿨타임
    private bool canAreaAttack = true;

    [Header("공격버프 설정")]
    [SerializeField] private int buffMpCost = 15;         //공격버프 소모마나량
    [SerializeField] private int buffAmount = 10;         //공격버프량 +10
    [SerializeField] private float buffDuration = 10.0f;  //공격버프 지속시간
    [SerializeField] private float buffCooldown = 30.0f;  //공격버프 쿨타임
    private bool isBuff;
    private bool canBuff = true;

    [Header("무적스킬 설정")]
    [SerializeField] private int invinMpCost = 30;        //무적기 소모마나량
    [SerializeField] private float invinDuration = 3.0f;  //무적기 지속시간
    [SerializeField] private float invinCooldown = 30.0f; //무적기 쿨타임
    private bool isInvin;
    private bool canInvin = true;

    [Header("몬스터 레이어")]
    [SerializeField] private LayerMask enemyLayer;

    private PlayerStatus playerStatus;
    private PlayerMovement playerMove;
    private PlayerHealth playerHealth;
    private PlayerSkill playerSkill;

    //스킬 쿨타임 UI
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
    }

    #region 기본공격
    //기본공격 : 공격범위 안에서 가장 앞에 있는 몬스터 1마리 공격
    public void Attack()
    {
        // 박스범위 안 enemy레이어를 가진 몬스터 콜라이더 리스트
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayer);
        if (hits.Length == 0) return;

        //몬스터 1마리 추출
        Collider2D target = GetTarget(hits);
        if (target == null) return;

        IDamageable damageable = target.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
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
    #endregion

    #region 범위공격
    //범위공격 : 공격 범위 안 몬스터들 모두 공격
    public void AreaAttack()
    {
        if (!playerSkill.AreaAttackUnlocked) return;
        if (!canAreaAttack) return;
        if (!playerStatus.UseMp(areaAttackMpCost)) return;

        //범위공격스킬 쿨타임
        StartCoroutine(AreaAttackCooldownCo());

        // 박스범위 안 enemy레이어를 가진 몬스터 콜라이더 리스트
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayer);
        if (hits.Length == 0) return;

        //해당 범위의 몬스터들 공격
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
        
        //쿨타임 설정
        areaAttackCooldownRemain = areaAttackCooldown;
        while (areaAttackCooldownRemain > 0)
        {
            areaAttackCooldownRemain -= Time.deltaTime;
            yield return null;
        }
        
        areaAttackCooldownRemain = 0;
        canAreaAttack = true;
    }
    #endregion

    #region 공격버프
    //공격버프 : 공격력을 10초간 +10 해준다. 쿨타임은 30초
    public void Buff()
    {
        if (!playerSkill.BuffUnlocked) return;
        if (!canBuff) return;
        if (isBuff) return;
        if (!playerStatus.UseMp(buffMpCost)) return;

        StartCoroutine(BuffCo());
        StartCoroutine(BuffCooldownCo());
    }
    IEnumerator BuffCo()
    {
        isBuff = true;

        playerStatus.AddAttack(buffAmount);
        Debug.Log("공격버프 시작");

        buffDurationRemain = buffDuration;

        while (buffDurationRemain > 0)
        {
            buffDurationRemain -= Time.deltaTime;
            yield return null;
        }

        buffDurationRemain = 0;

        playerStatus.RemoveAttack(buffAmount);
        isBuff = false;

        Debug.Log("공격버프 종료");
    }
    IEnumerator BuffCooldownCo()
    {
        canBuff = false;

        buffCooldownRemain = buffCooldown;

        while (buffCooldownRemain > 0)
        {
            buffCooldownRemain -= Time.deltaTime;
            yield return null;
        }
        buffCooldownRemain = 0;
        canBuff = true;
        Debug.Log("공격버프 사용가능");
    }
    #endregion

    #region 무적기
    // 무적기 : 플레이어는 3초동안 데미지를 받지 않는다. 쿨타임은 30초
    public void Invin()
    {
        if (!playerSkill.InvinUnlocked) return;
        if (!canInvin) return;
        if (isInvin) return;
        if (!playerStatus.UseMp(invinMpCost)) return;

        StartCoroutine(InvinCo());
        StartCoroutine(InvinCooldownCo());
    }
    IEnumerator InvinCo()
    {
        isInvin = true;

        playerHealth.SetInvin(true);
        Debug.Log("무적기 시작");

        yield return new WaitForSeconds(invinDuration);

        playerHealth.SetInvin(false);
        Debug.Log("무적기 종료");

        isInvin = false;
    }
    IEnumerator InvinCooldownCo()
    {
        canInvin = false;

        invinCooldownRemain = invinCooldown;

        while (invinCooldownRemain > 0)
        {
            invinCooldownRemain -= Time.deltaTime;
            yield return null;
        }
        invinCooldownRemain = 0;
        canInvin = true;
        Debug.Log("무적기 사용가능");
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(attackPoint.position, attackSize);
        }
    }
}
