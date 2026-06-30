using UnityEngine;

public class ShootingPoint : MonoBehaviour
{
    [SerializeField] Boss boss;



    private void Awake()
    {
        if(boss == null)
        {
            boss = GetComponentInParent<Boss>();
        }
        
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if( boss.Direction)
        {
            transform.localPosition = new Vector3(0.6f, 0.2f, 0);
        }

        else
        {
            transform.localPosition = new Vector3(-0.6f, 0.2f, 0);
        }
    }
}
