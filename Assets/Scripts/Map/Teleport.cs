using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("텔레포트")]
    [SerializeField] private Transform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //텔레포트 트리거에 Player 가 닿았다면
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = target.position;
        }
    }
}
