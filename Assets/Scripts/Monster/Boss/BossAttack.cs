using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform droppingPoint;
    [SerializeField] private float attackDuration = 5.0f;
    [SerializeField] private float attackCoolTime = 1.0f;
    [SerializeField] private float shootCoolTime = 1.0f;
    [SerializeField] private float dropCoolTime = 1.0f;
    [SerializeField] private float throwCoolTime = 1.0f;
    [SerializeField] private BossBullet bulletPrefab;
    [SerializeField] private Drop dropPrefab;
    [SerializeField] private PoisonBall poisonBallPrefab;
    [SerializeField] private float dropSpace = 1.0f;
    [SerializeField] private BossAnimation bossAnimation;

    [SerializeField] private GameObject magicCreatePrefab;

    private int randomInt;
    private Coroutine currentAttack;

    

    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossAnimation = GetComponent<BossAnimation>();
        shootingPoint = transform.Find("ShootingPoint");
        droppingPoint = transform.Find("DroppingPoint");
    }

    private void Start()
    {
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        while (true)
        {
            bossAnimation.Attack();
            randomInt = Random.Range(0, 3);
            StartAttack(randomInt);
            yield return new WaitForSeconds(attackDuration);
            EndAttack();
            yield return new WaitForSeconds(attackCoolTime);
        }
    }

    IEnumerator ShootingCo()
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(shootCoolTime);
        }
    }

    IEnumerator DroppingCo()
    {
        while (true)
        {
            
            Drop(boss.Direction);
            yield return new WaitForSeconds(dropCoolTime);
        }
    }

    IEnumerator ThrowingCo()
    {
        while (true)
        {
            ThrowPoison();
            yield return new WaitForSeconds(throwCoolTime);
        }
    }
    
    private void StartAttack(int random)
    {
        switch (random)
        {
            case 0: 
                currentAttack = StartCoroutine(ShootingCo());
                break;

            case 1:
                currentAttack = StartCoroutine(DroppingCo());
                break;
            case 2:
                currentAttack = StartCoroutine(ThrowingCo());
                break;
        }

    }

    private void EndAttack()
    {
        if(currentAttack != null)
        {
            StopCoroutine(currentAttack);
            currentAttack = null;
        }
    }

    private void Shoot()
    {
        HorizontalMagicCreate();
        BossBullet bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.Init(boss);
        
    }

    private void Drop(bool dir)
    {

        if (dropSpace > 5.0f)
        {
            dropSpace = 1.0f;
        }

        if(dir)
        {
            Instantiate(dropPrefab, droppingPoint.position + Vector3.right * dropSpace, Quaternion.identity);
            Instantiate(magicCreatePrefab, droppingPoint.position + Vector3.right * dropSpace, Quaternion.identity);
        }
        else
        {
            Instantiate(dropPrefab, droppingPoint.position + Vector3.left * dropSpace, Quaternion.identity);
            Instantiate(magicCreatePrefab, droppingPoint.position + Vector3.left * dropSpace, Quaternion.Euler(0, 0, 90));
        }
        dropSpace++;
    }

    private void ThrowPoison()
    {
        HorizontalMagicCreate();
        PoisonBall poisonBall = Instantiate(poisonBallPrefab, shootingPoint.position, Quaternion.identity);
        poisonBall.Init(boss);
    }

    private void HorizontalMagicCreate()
    {
        if (boss.Direction)
        {
            Instantiate(magicCreatePrefab, shootingPoint.position, Quaternion.identity);
        }
        
        else
        {
            Instantiate(magicCreatePrefab, shootingPoint.position, Quaternion.Euler(0, 0, 180));
        }
    }
}
