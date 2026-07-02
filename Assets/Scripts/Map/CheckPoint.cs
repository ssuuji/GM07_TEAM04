using Unity.Cinemachine;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Collider2D cameraBound;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private CinemachineConfiner2D confiner;

    private void Start()
    {
        confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 체크포인트 트리거 안에 들어왔다면 해당위치 저장
        if (collision.TryGetComponent(out PlayerRespawn respawn))
        {
            //현재위치를 체크포인트로
            respawn.SetRespawnPoint(transform);
            GameSceneManager.Instance.SetCheckPoint(transform.position); //RETRY용 위치저장 

            // 해당 구역 카메라 범위 적용
            confiner.BoundingShape2D = cameraBound;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
