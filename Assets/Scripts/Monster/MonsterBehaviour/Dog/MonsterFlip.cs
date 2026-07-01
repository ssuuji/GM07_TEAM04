using UnityEngine;

public class MonsterFlip : MonoBehaviour
{
    [SerializeField] private Monster monster;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }


    private void Update()
    {
        FlipSet();
    }

    private void FlipSet()
    {
        transform.localScale = new Vector3(monster.Direction ? 1 : -1, 1, 1);
    }
}
