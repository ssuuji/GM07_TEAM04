using UnityEngine;

public class Monster : MonoBehaviour
{
    
    enum MonsterState
    {
        None = -1, Idle, Angry
    }

    private MonsterState currentState;

    [Header("몬스터 상태")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private MonsterIdleMove monsterIdleMove;
    [SerializeField] private MonsterAngryMove monsterAngryMove;

    public bool IsDead { get; private set; } = false;
    private int currentHealth;

    

    private void OnEnable()
    {
        currentHealth = maxHealth;
        currentState = MonsterState.Idle;
    }

    private void Update()
    {

        if(currentState == MonsterState.Idle)
        {
            monsterIdleMove.enabled = true; //일반모드 O
            monsterAngryMove.enabled = false; //추격모드 X
            if (currentHealth < maxHealth)
            {
                SetState(MonsterState.Angry);
            }
        }

        else if (currentState == MonsterState.Angry)
        {
            monsterIdleMove.enabled = false; //일반모드 X
            monsterAngryMove.enabled = true; //추격모드 O
        }

        
    }

    //피해
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
        
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
    }

    //테스트용 충돌시 데미지 피해
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }
}
