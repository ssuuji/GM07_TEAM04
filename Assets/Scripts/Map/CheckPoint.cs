using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 체크포인트 트리거 안에 들어왔다면 해당위치 저장
        if (collision.TryGetComponent(out PlayerRespawn respawn))
        {
            //현재위치를 체크포인트로
            respawn.SetRespawnPoint(transform);
        }
    }
}
