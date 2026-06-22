using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;


    

    public bool Direction = false;


    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void UpdateDirection()
    {
        Direction = transform.position.x >= player.transform.position.x ? true : false;
    }
}
