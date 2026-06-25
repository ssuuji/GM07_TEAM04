using UnityEngine;

public class BossFlip : MonoBehaviour
{
    [SerializeField] private Boss boss;

    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }


    private void Update()
    {
        FlipSet();
    }

    private void FlipSet()
    {
        transform.localScale = new Vector3(boss.Direction ? 1 : -1, 1, 1);
    }
}
