using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int countPerWave = 4;
    [SerializeField] private int waveCount = 4;
    [SerializeField] private float waveCoolTime = 10.0f;
    [SerializeField] private float widthMin = -10.0f;
    [SerializeField] private float widthMax = 10.0f;
    [SerializeField] private float heightMin = 0.0f;
    [SerializeField] private float heightMax = 10.0f;

    private Vector3 randomPosition;

    private void Start()
    {
        StartCoroutine(WaveCo());
    }

    IEnumerator WaveCo()
    {
        while (true)
        {
            if(waveCount <= 0)
            {
                yield break;
            }

            waveCount--;

            CreateWave();
            yield return new WaitForSeconds(waveCoolTime);
        }
    }


    private void CreateWave()
    {
        for (int i = 0; i < countPerWave; i++)
        {
            SetRandomPosition();
            Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
        }
        
    }

    private void SetRandomPosition()
    {
        randomPosition = new Vector3(
            Random.Range(widthMin, widthMax),
            Random.Range(heightMin, heightMax),
            0f);
    }
}
