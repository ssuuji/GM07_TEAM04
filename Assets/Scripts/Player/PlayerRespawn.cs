using Demo_Project;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("리스폰 위치")]
    private Transform respawnPoint;
    private MessageUI messageUI;


    [SerializeField] private CinemachineCamera cinemachineCamera;
    private Vector3 warpDistance;
    private bool isRetryRespawn;

    private void Awake()
    {
        GameSceneManager gameSceneManager = GameSceneManager.Instance;

        if (gameSceneManager == null)
        {
            return;
        }

        // Retry로 게임씬을 다시 재생한 경우
        if (GameSceneManager.Instance.IsCheckPointSave)
        {
            Vector3 beforePos = transform.position;
            Vector3 checkPointPos = GameSceneManager.Instance.lastCheckPoint;

            transform.position = checkPointPos;

            warpDistance = checkPointPos - beforePos;
            isRetryRespawn = true;
        }
    }
    private void Start()
    {
        messageUI = FindFirstObjectByType<MessageUI>();

        if (isRetryRespawn)
        {
            StartCoroutine(CameraWarpCo());
        }
    }

    private IEnumerator CameraWarpCo()
    {
        yield return null;

        if (cinemachineCamera != null)
        {
            CinemachineCore.OnTargetObjectWarped(transform, warpDistance);
            cinemachineCamera.PreviousStateIsValid = false;
        }
    }

    //리스폰 위치 변경
    public void SetRespawnPoint(Transform point)
    {
        if (respawnPoint == point) return;

        respawnPoint = point;
        messageUI.ShowMessage("체크포인트 저장!");
    }

    //리스폰
    public void Respawn()
    {
        if (respawnPoint == null) return;

        transform.position = respawnPoint.position;
    }
}
