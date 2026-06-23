using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;


    private float currentHealth;

    public bool Direction = false;

    private void Awake()
    {
        currentHealth = maxHealth;
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
    }

    private void UpdateDirection()
    {
        Direction = (transform.position.x <= player.transform.position.x) ? true : false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        //if (bossUI != null)
        //{
        //    bossUI.SetHP(currentHealth);
        //}
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
