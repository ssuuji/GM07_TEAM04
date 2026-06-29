using UnityEngine;

public class Cloning : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private int cloneCount = 4;
    [SerializeField] private float spawnWidth = 10.0f;
    [SerializeField] private float spawnHeight = 5.0f;
    [SerializeField] private GameObject cloningEffectPrefab;

    private Vector3[] randomSpots;
    private Boss[] clones;
    

    private void Awake()
    {
        boss = GetComponent<Boss>();
        randomSpots = new Vector3[cloneCount];
        clones = new Boss[cloneCount];
    }

    public void UseCloning()
    {
        Instantiate(cloningEffectPrefab, transform.position, Quaternion.identity);
        MakeRandomSpot();
        SpawnClones();
        SetAllHp();

    }

    

    private void MakeRandomSpot()
    {
        for (int i = 0; i < randomSpots.Length; i++)
        {
            randomSpots[i] = transform.position + new Vector3(Random.Range(-spawnWidth, spawnWidth), Random.Range(0, spawnHeight), 0f);
        }
        boss.transform.position = transform.position + new Vector3(Random.Range(-spawnWidth, spawnWidth), Random.Range(0, spawnHeight), 0f);
    }

    private void SpawnClones()
    {
        for (int i = 1; i < randomSpots.Length; i++)
        {
            clones[i] = Instantiate(bossPrefab, randomSpots[i], Quaternion.identity);
        }
        
    }

    private void SetAllHp()
    {
        //본체
        clones[0] = boss;
        for (int i = 1; i < cloneCount; ++i)
        {
            clones[i].SetHp(1);
            clones[i].IsFake = true;
        }
    }
}
