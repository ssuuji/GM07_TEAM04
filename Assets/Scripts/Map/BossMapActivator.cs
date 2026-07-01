using UnityEngine;

public class BossMap : MonoBehaviour
{
    [SerializeField] private GameObject boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.SetActive(true); //보스활성화
        gameObject.SetActive(false); //포탈 비활성화
    }
}
