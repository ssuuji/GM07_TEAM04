using UnityEngine;
using Unity.Cinemachine;

public class Teleport : MonoBehaviour
{
    [Header("텔레포트")]
    [SerializeField] private Transform target;

    [Header("다음맵 카메라 범위")]
    [SerializeField] private Collider2D cameraBound;              //다음맵 범위
    [SerializeField] private CinemachineCamera cinemachineCamera; //시네머신
    private CinemachineConfiner2D confiner;                       //시네머신 Confiner2D

    private void Start()
    {
        confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
    }

    public void TeleportPlayer(Transform player)
    {
        if (player == null || target == null) return;

        //다음맵 카메라 범위 설정
        if (cameraBound != null && confiner != null)
        {
            confiner.BoundingShape2D = cameraBound;  // 카메라 범위를 변경하고
            confiner.InvalidateBoundingShapeCache(); // 변경된 범위로 다시 계산적용
        }

        //카메라도 순간이동 처리 (기존위치와 타겟위치의 거리를 계산하여 카메라도 그 거리만큼 바로 이동)
        Vector3 beforePos = player.position;                    // 기존 위치 저장
        player.position = target.position;                      // 플레이어를 타겟 위치로 이동
        Vector3 distance = player.position - beforePos;         // 순간이동한 거리 계산
        CinemachineCore.OnTargetObjectWarped(player, distance); // Cinemachine에게 플레이어가 이동한 거리만큼 이동하라고 알림
        cinemachineCamera.PreviousStateIsValid = false;         // 이전 카메라 상태를 꺼서 화면이 삐그덕거리는거 해결

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction interact))
        {
            interact.SetInPortal(this, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerInteraction interact))
        {
            interact.SetInPortal(this, false);
        }
    }
}
