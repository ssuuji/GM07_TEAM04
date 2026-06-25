using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Chest : MonoBehaviour
{
    [Header("상자 애니메이션")]
    [SerializeField] private Animator chestAnim;
    private bool isOpen;

    [Header("보상")] // 랜덤무기, 랜덤골드, 랜덤능력치?
    [SerializeField] private Transform rewardPoint; //보상포인트(위치)
    [SerializeField] private float rewardInterval = 0.2f;  //보상간격
    [SerializeField] private List<GameObject> rewardPrefabs = new List<GameObject>(); //보상 리스트
    [SerializeField] private int minGold = 0;       //min ~ max 사이의 골드를 랜덤으로 지급
    [SerializeField] private int maxGold = 100;

    private void Start()
    {
        //우선 닫힌 상자 모습으로 보이게끔 애니메이션 비활성화
        if (chestAnim != null)
        {
            chestAnim.enabled = false;
        }
    }

    //상자 오픈 : Chest 애니메이션 활성화
    public void ChestOpen()
    {
        if (isOpen) return; //이미 열린 상자는 다시 열리지 않도록

        isOpen = true;
        if (chestAnim != null)
        {
            chestAnim.enabled = true; //상자 애니메이션 활성화
            chestAnim.Play("ChestOpen", 0, 0f);
        }

        Debug.Log("상자오픈");

        //상자 보상
        StartCoroutine(RewardCo());
    }

    IEnumerator RewardCo()
    {
        yield return new WaitForSeconds(rewardInterval);

        RandReward(); //랜덤 보상 1개
        RandGold();   //랜덤 골드
    }

    private void RandReward()
    {
        if (rewardPrefabs.Count == 0 || rewardPoint == null) return;

        // 랜덤보상뽑기
        int randomReward = Random.Range(0, rewardPrefabs.Count);
        GameObject rewardPrefab = rewardPrefabs[randomReward];

        //보상 지급
        Instantiate(rewardPrefab, rewardPoint.position, Quaternion.identity, rewardPoint);
    }

    private void RandGold()
    {
        // 랜덤 골드뽑기
        int randomGold = Random.Range(minGold, maxGold + 1);

        // 골드 지급
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(randomGold);
            Debug.Log($"골드 {randomGold} 획득!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction interact))
        {
            interact.SetInChest(this, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction interact))
        {
            interact.SetInChest(this, false);
        }
    }
}
