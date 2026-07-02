using Unity.Cinemachine;
using UnityEngine;

public class BossMap : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private CinemachineCamera cinemachineCamera;

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

        //보스맵에서 카메라 Follow 끊기
        cinemachineCamera.Follow = null;
        cinemachineCamera.PreviousStateIsValid = false;

        gameObject.SetActive(false); //포탈 비활성화
    }
}
