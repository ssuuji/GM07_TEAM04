using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject player;

    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;
    [SerializeField] Cloning cloning;

    [SerializeField] private bool flair;
    //테스트용
    private float flairTime = 1.0f;

    public bool IsFake = false;
    public bool Direction = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        cloning = GetComponent<Cloning>();  
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
        currentHealth -= damage;


        if (currentHealth % 35 == 0 && IsFake == false)
        {
            cloning.UseCloning();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        //if (bossUI != null)
        //{
        //    bossUI.SetHP(currentHealth);
        //}
    }

    public void SetHp(int hp)
    {
        currentHealth = hp;
    }

    public int GetHpInfo()
    {
        return currentHealth;
    }


    private void Die()
    {
        Destroy(gameObject);
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
