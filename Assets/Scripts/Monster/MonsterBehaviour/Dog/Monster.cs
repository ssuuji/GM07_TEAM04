using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    

    enum MonsterState
    {
        None = -1, Idle, Angry, Knockback
    }

    private MonsterState currentState;

    [Header("몬스터 상태")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackTime = 1.0f;
    [SerializeField] private MonsterIdleMove monsterIdleMove;
    [SerializeField] private MonsterAngryMove monsterAngryMove;
    [SerializeField] private MonsterUI monsterUI;
    [SerializeField] private MonsterKnockBack monsterKnockBack;
    [SerializeField] private DogAnimation dogAnimation;
    [SerializeField] private MonsterAttack monsterAttack;

    [SerializeField] private UIAppearance ui;

    private Rigidbody2D rb;

    private MonsterReward monsterReward;

    private float flairTime = 3;
    private float knockbackTimer;
    public bool IsDead { get; private set; } = false;
    public bool Direction { get; private set; } = true;

    private bool isReturned = true;

    public void Initialize()
    {
        currentHealth = maxHealth;
        isReturned = false;
        IsDead = false;
        if (monsterUI != null)
        {
            monsterUI.Initialize(maxHealth);
        }

        ui.Disappear();

        monsterReward = GetComponent<MonsterReward>();
        monsterAttack = GetComponent<MonsterAttack>();

        SetKnockbackTime();
        SetState(MonsterState.Idle);
        
        monsterAttack.enabled = false;
    }

    private void Awake()
    {
        dogAnimation = GetComponent<DogAnimation>();
        ui = GetComponent<UIAppearance>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {

        StateManage();
        
    }

    //피해
    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        SFXManager.Instance.PlayMonsterHit();


        if (monsterUI != null)
        {
            monsterUI.SetHP(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        SetState(MonsterState.Knockback);
        ui.Appear();
        monsterKnockBack.Knockback();
    }

    //죽음
    private void Die()
    {
        if(IsDead) return; 
        IsDead = true;
        StartCoroutine(DieCo());
        
    }

    IEnumerator DieCo()
    {
        dogAnimation.Die();
        SFXManager.Instance.PlayMonsterDie();
        yield return new WaitForSeconds(1f);
        ReturnToPool();

        monsterReward.DropReward();
        GameManager.Instance?.AddKillCount();
    }

    private void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }
        isReturned = true;
        rb.linearVelocity = Vector2.zero;
        Managers.Pool.ReturnPool(this);

    }


    //상태
    private void SetState(MonsterState state)
    {

        if (IsDead) return;
        currentState = state;

        monsterIdleMove.enabled = currentState == MonsterState.Idle;
        monsterAngryMove.enabled = currentState == MonsterState.Angry;
        monsterKnockBack.enabled = currentState == MonsterState.Knockback;

    }





    private void StateManage()
    {

        

        if (currentState == MonsterState.Knockback)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                SetState(MonsterState.Angry);
                SetKnockbackTime();
            }

            return;
        }

        if (currentHealth < maxHealth)
        {
            SetState(MonsterState.Angry);
            
        }

        else
        {
            SetState(MonsterState.Idle);
        }
    }

    private void SetKnockbackTime()
    {
        knockbackTimer = knockbackTime;
    }
    

    //방향설정
    public void SetDirection(bool dir)
    {
        Direction = dir;
    }

    //테스트용
    private void Flair()
    {
        if (flairTime <= 0)
        {
            TakeDamage(1);  
            flairTime = 3;
        }
        else
        {
            flairTime -= Time.deltaTime;
        }
    }
    
}
