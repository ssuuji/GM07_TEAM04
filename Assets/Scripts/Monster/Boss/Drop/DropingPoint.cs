using UnityEngine;

public class DropingPoint : MonoBehaviour
{
    [SerializeField] private Boss boss;
    

    private void Awake()
    {
        if (boss == null)
        {
            boss = GetComponent<Boss>();
        }

    }

    private void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        if (boss.Direction)
        {
            transform.localPosition = new Vector3(1, 6, 0);
        }

        else
        {
            transform.localPosition = new Vector3(-1, 6, 0);
        }
    }
}
