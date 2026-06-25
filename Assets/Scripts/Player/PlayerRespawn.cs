using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("리스폰 위치")]
    private Transform respawnPoint;

    //리스폰 위치 변경
    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    //리스폰
    public void Respawn()
    {
        if (respawnPoint == null) return;

        transform.position = respawnPoint.position;
    }
}
