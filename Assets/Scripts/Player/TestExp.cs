using UnityEngine;

public class TestExp : MonoBehaviour
{
    //테스트 용 ... 경험치
    [SerializeField] private int expAmount = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLevel level = collision.GetComponent<PlayerLevel>();

        level.AddExp(expAmount);

        Destroy(gameObject);
    }

    
}
