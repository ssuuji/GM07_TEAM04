using UnityEngine;

public class TestExp : MonoBehaviour
{
    [SerializeField] private int expAmount = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLevel level = collision.GetComponent<PlayerLevel>();

        level.AddExp(expAmount);

        Destroy(gameObject);
    }

    
}
