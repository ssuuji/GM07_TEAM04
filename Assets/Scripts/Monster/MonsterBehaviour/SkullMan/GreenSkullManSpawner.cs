using System.Collections;
using UnityEngine;

public class GreenSkullManSpawner : MonoBehaviour
{
    [SerializeField] private SkullMan skullManPrefab;
    [SerializeField] private int skullManCountMax = 10;
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

    // 웨이브 생성
    IEnumerator WaveCo()
    {
        while (waveCount > 0)
        {

            while (CountSkullMan() > skullManCountMax)
            {
                yield return null;
            }

            waveCount--;

            CreateWave();

            yield return new WaitForSeconds(waveCoolTime);
        }
    }

    // 웨이브 한개
    private void CreateWave()
    {
        for (int i = 0; i < countPerWave; i++)
        {
            SetRandomPosition();
            SkullMan skullMan = Managers.Pool.GetPool(skullManPrefab);

            skullMan.transform.position = randomPosition;
            skullMan.transform.rotation = Quaternion.identity;

            if (skullMan != null)
            {
                skullMan.Initialize();
            }
        }

    }

    // 범위 내 랜덤한 위치
    private void SetRandomPosition()
    {
        randomPosition = new Vector3(
            transform.position.x + Random.Range(widthMin, widthMax),
            transform.position.y + Random.Range(heightMin, heightMax),
            0f);
    }

    private int CountSkullMan()
    {
        return GameObject.FindGameObjectsWithTag("GreenSkullMan").Length;
    }
}
