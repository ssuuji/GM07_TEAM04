using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootCoolTime = 1.0f;
    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        shootingPoint = transform.Find("ShootingPoint");
    }

    private void Start()
    {
        StartCoroutine(ShootingCo());
    }

    IEnumerator ShootingCo()
    {
        while(true)
        {
            Shoot();
            yield return new WaitForSeconds(shootCoolTime);
        }
    }


    private void Shoot()
    {
        Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
    }
}
