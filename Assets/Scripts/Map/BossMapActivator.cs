using UnityEngine;

public class BossMap : MonoBehaviour
{
    [SerializeField] private GameObject boss;

    [Header("Boss Map BGM")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bossBGM;         

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.SetActive(true);        //보스활성화

        audioSource.Stop();

        //보스맵 BGM 재생
        audioSource.clip = bossBGM;
        audioSource.loop = true;
        audioSource.Play();

        gameObject.SetActive(false); //포탈 비활성화
    }
}
