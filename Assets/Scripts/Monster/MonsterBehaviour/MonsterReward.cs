using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonsterReward : MonoBehaviour
{
    //몬스터처지 보상리스트 관리
    [Header("보상리스트")]
    [SerializeField] private List<GameObject> rewardPrefabs = new List<GameObject>();

    [Header("경험치")]
    [SerializeField] private int expReward = 10;
    private PlayerLevel playerLevel;

    private float dropJumpPower = 0.8f;
    private float rewardPopDuration = 0.35f;
    private float dropRangeX = 0.7f;

    private void Start()
    {
        playerLevel = FindFirstObjectByType<PlayerLevel>();
    }

    //몬스터 사망 시 호출
    public void DropReward()
    {
        GiveExp();    //경험치 
        DropItems();  //아이템 or 골드
    }

    private void GiveExp()
    {
        if (playerLevel != null)
        {
            playerLevel.AddExp(expReward);
        }
    }

    private void DropItems()
    {
        if (rewardPrefabs.Count == 0)
        {
            return;
        }

        int maxDropCount = rewardPrefabs.Count;             //리스트 총개수를 max값으로
        int dropCount = Random.Range(1, maxDropCount + 1);  //보상 몇 개를 드랍할지 랜덤으로뽑기

        for (int i = 0; i < dropCount; i++)
        {
            int randomIndex = Random.Range(0, rewardPrefabs.Count);
            GameObject dropPrefab = rewardPrefabs[randomIndex];
            SpawnDrop(dropPrefab);
        }
    }

    private void SpawnDrop(GameObject dropPrefab)
    {
        if (dropPrefab == null)
        {
            return;
        }

        GameObject dropObject = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Collider2D dropCollider = dropObject.GetComponentInChildren<Collider2D>();
        if (dropCollider != null) dropCollider.enabled = false;

        Vector3 originScale = dropObject.transform.localScale;
        dropObject.transform.localScale = originScale * 0.3f;

        Vector3 targetPosition = transform.position + new Vector3(Random.Range(-dropRangeX, dropRangeX), 0f, 0f);

        Sequence dropSequence = DOTween.Sequence();
        dropSequence.Join(dropObject.transform.DOJump(targetPosition, dropJumpPower, 1, rewardPopDuration).SetEase(Ease.OutQuad));

        dropSequence.Join(dropObject.transform.DOScale(originScale, rewardPopDuration * 0.5f).SetEase(Ease.OutBack));

        dropSequence.OnComplete(() =>
        {
            if (dropCollider != null)
            {
                dropCollider.enabled = true;
            }
        });
    }
}