using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject player;

    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;
    [SerializeField] Cloning cloning;

    [SerializeField] private BossUI bossUI;

    [SerializeField] private GameObject diePrefab;

    [SerializeField] private bool flair;

    [SerializeField] private BossAnimation bossAnimation;

    [SerializeField] private SFXManager sfxManager;

    [SerializeField] private GameObject clearPortal; //보스 클리어 후 등장하는 포탈

    //테스트용
    private float flairTime = 1.0f;

    private int cloningCount = 2;

    public bool IsFake = false;
    public bool Direction = false;

    public bool IsDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        cloning = GetComponent<Cloning>();
        bossAnimation = GetComponent<BossAnimation>();
        bossUI = GetComponent<BossUI>();
        bossUI.Initialize(maxHealth);

    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void Update()
    {
        UpdateDirection();

        if(flair)
        {
            //테스트용
            Flair();
        }
        
    }

    private void UpdateDirection()
    {
        Direction = (transform.position.x <= player.transform.position.x) ? true : false;
    }


    public void TakeDamage(int damage)
    {
        SFXManager.Instance.PlayHit();

        currentHealth -= damage;
        bossAnimation.Hit();

        if (currentHealth <= maxHealth * 0.8f  && IsFake == false && cloningCount == 2)
        {
            cloning.UseCloning();
            cloningCount--;
        }

        else if(currentHealth <= maxHealth * 0.4f && IsFake == false && cloningCount == 1)
        {
            cloning.UseCloning();
            cloningCount--;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (bossUI != null)
        {
            bossUI.SetHP(currentHealth);
        }
    }

    public void SetHp(int hp)
    {
        currentHealth = hp;
    }

   


    private void Die()
    {
        if (IsDead) return;
        IsDead = true;

        SFXManager.Instance.PlayBossDie();

        
        StartCoroutine(DieCo());
    }

    IEnumerator DieCo()
    {
        bossAnimation.Die();

        yield return new WaitForSeconds(1);
        Instantiate(diePrefab, transform.position, Quaternion.identity);

        //분신이면 포탈을 열지 않고 그냥 제거
        if (IsFake)
        {
            Destroy(gameObject);
            yield break;
        }

        //보스라면 살아있는 분신이 전부 사라질 때까지 대기
        while (HasAliveClone())
        {
            yield return null;
        }

        Destroy(gameObject);

        //모두 제거 했다면 포탈 활성화
        clearPortal.SetActive(true);
    }

    //분신 체크용
    private bool HasAliveClone()
    {
        Boss[] bosses = FindObjectsByType<Boss>(FindObjectsSortMode.None);

        foreach (Boss boss in bosses)
        {
            if (boss != this && boss.IsFake && !boss.IsDead)
            {
                return true;
            }
        }

        return false;
    }

    //테스트용
    private void Flair()
    {
        if (flairTime <= 0)
        {
            TakeDamage(10);
            flairTime = 1;
        }
        else
        {
            flairTime -= Time.deltaTime;
        }
    }
}
