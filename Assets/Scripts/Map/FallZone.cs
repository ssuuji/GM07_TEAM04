using UnityEngine;

public class FallZone : MonoBehaviour
{
    //맵 하단 낙사구간
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerRespawn respawn))
        {
            respawn.Respawn();
        }
    }
}
