using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("리스폰 위치")]
    private Transform respawnPoint;
    private MessageUI messageUI;

    private void Start()
    {
        messageUI = FindFirstObjectByType<MessageUI>();
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
