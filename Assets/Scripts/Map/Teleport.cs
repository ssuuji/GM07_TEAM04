using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("텔레포트")]
    [SerializeField] private Transform target;
    private Transform player; //플레이어 정보
    private bool isInside;    //포탈범위 안에 있는지 확인여부

    private void Update()
    {
        //포탈범위에 있고 상호작용키를 눌렀다면
        if (isInside && InputManager.IsInteract)
        {
            if (player != null)
            {
                //플레이어를 타겟포탈로 이동
                player.position = target.position;
                
                //초기화
                isInside = false;
                player = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //포탈 범위에 Player 가 닿았다면
        if (collision.CompareTag("Player"))
        {
            //입장 허용
            isInside = true;
            //플레이어 위치정보 저장
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //포탈 범위를 벗어났다면 상태 초기화
        if (collision.CompareTag("Player"))
        {
            isInside = false;
            player = null;
        }
    }
}
