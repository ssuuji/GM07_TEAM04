using UnityEngine;

public class PoisonFloor : MonoBehaviour
{
    [SerializeField] PlayerHealth player;
    [SerializeField] private int damage = 5;
    [SerializeField] private float damegeCoclTime = 1.0f;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private GameObject hitFxPrefab;

    private float currentCoolTime;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        Aging();
    }

    private void Aging()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            currentCoolTime -= Time.deltaTime;

            if(currentCoolTime <= 0)
            {
                Instantiate(hitFxPrefab, player.transform.position, Quaternion.identity);
                player.TakeDamage(damage);
                currentCoolTime = damegeCoclTime;
            }
        }
    }
}


