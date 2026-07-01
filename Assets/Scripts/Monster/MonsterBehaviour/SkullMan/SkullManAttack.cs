using Unity.VisualScripting;
using UnityEngine;

public class SkullManAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCoolTime = 1f;
    
    [SerializeField] private Transform attackPoint;
    [SerializeField] private SkullMan skullMan;
    [SerializeField] private PlayerHealth player;

    [SerializeField] private GameObject skullManAttackHitPrefab;
    [SerializeField] private GameObject skullManAttackPrefab;
    [SerializeField] private DogAnimation dogAnimation;

    private float currentCoolTime;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }


    private void Update()
    {
        if (currentCoolTime <= 0)
        {
            Attack();
            
            currentCoolTime = attackCoolTime;
        }

        else
        {
            currentCoolTime -= Time.deltaTime;
        }
    }

    private void Attack()
    {
            //dogAnimation.Attack();
            SFXManager.Instance.PlaySkullManAttack();
            Instantiate(skullManAttackPrefab, attackPoint.position, Quaternion.identity);
            Instantiate(skullManAttackHitPrefab, player.transform.position + Vector3.up * 2, Quaternion.identity);
            player.TakeDamage(damage);
    }
}
