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


    private float flairTime = 3;
    private float knockbackTimer;
    private float attackTimer;
    public bool IsDead { get; private set; } = false;
    public bool Direction { get; private set; } = true;
    
    

    private void OnEnable()
    {
        currentHealth = maxHealth;
        
        SetKnockbackTime();
    }

    private void Start()
    {
        if (monsterUI != null)
        {
            monsterUI.Initialize(maxHealth);
        }
    }


    private void Update()
    {
        Debug.Log("MonDir = " + Direction);


        StateManage();
        
    }

    //피해
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        if (monsterUI!=null)
        {
            monsterUI.SetHP(currentHealth);
        }
        SetState(MonsterState.Knockback);
        monsterKnockBack.Knockback();
    }

    //죽음
    private void Die()
    {
        IsDead = true;
        Destroy(gameObject);
    }


    //상태
    private void SetState(MonsterState state)
    {
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
