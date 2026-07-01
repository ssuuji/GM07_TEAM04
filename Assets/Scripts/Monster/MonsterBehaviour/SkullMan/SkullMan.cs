using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkullMan : MonoBehaviour, IDamageable
{
    
    enum SkullManState
    {
        None = -1, Idle, Angry, Knockback
    }

    private SkullManState currentState;

    [Header("몬스터 상태")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackTime = 1.0f;
    [SerializeField] private SkullManIdleMove skullManIdleMove;
    [SerializeField] private SkullManAngryMove skullManAngryMove;
    [SerializeField] private MonsterUI monsterUI;
    [SerializeField] private SkullManKnockBack skullManKnockBack;
    [SerializeField] private DogAnimation dogAnimation;

    [SerializeField] private UIAppearance ui;


    private MonsterReward monsterReward;

    private float flairTime = 3;
    private float knockbackTimer;
    public bool IsDead { get; private set; } = false;
    public bool Direction { get; private set; } = true;

    private void Awake()
    {
        dogAnimation = GetComponent<DogAnimation>();
        ui = GetComponent<UIAppearance>();
    }

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
        monsterReward = GetComponent<MonsterReward>();
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
        SFXManager.Instance.PlaySkullManHit();


        if (monsterUI != null)
        {
            monsterUI.SetHP(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        ui.Appear();
        SetState(SkullManState.Knockback);
        skullManKnockBack.Knockback();
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
        SFXManager.Instance.PlaySkullManDie();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        monsterReward.DropReward();
    }


    //상태
    private void SetState(SkullManState state)
    {

        if (IsDead) return;
        currentState = state;

        skullManIdleMove.enabled = currentState == SkullManState.Idle;
        skullManAngryMove.enabled = currentState == SkullManState.Angry;
        skullManKnockBack.enabled = currentState == SkullManState.Knockback;

    }





    private void StateManage()
    {

        

        if (currentState == SkullManState.Knockback)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                SetState(SkullManState.Angry);
                SetKnockbackTime();
            }

            return;
        }

        if (currentHealth < maxHealth)
        {
            SetState(SkullManState.Angry);
            
        }

        else
        {
            SetState(SkullManState.Idle);
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
