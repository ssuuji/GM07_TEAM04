using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform droppingPoint;
    [SerializeField] private float attackDuration = 5.0f;
    [SerializeField] private float attackCoolTime = 1.0f;
    [SerializeField] private float shootCoolTime = 1.0f;
    [SerializeField] private float dropCoolTime = 1.0f;
    [SerializeField] private float throwCoolTime = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private GameObject poisonBallPrefab;

    private int randomInt;
    private Coroutine currentAttack;

    

    private void Awake()
    {
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
            Drop();
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
        Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
    }

    private void Drop()
    {
        Instantiate(dropPrefab, droppingPoint.position, Quaternion.identity);
    }

    private void ThrowPoison()
    {
        Instantiate(poisonBallPrefab, shootingPoint.position, Quaternion.identity);
    }
}
