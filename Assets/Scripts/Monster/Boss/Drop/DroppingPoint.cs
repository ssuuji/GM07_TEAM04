using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DroppingPoint : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float movingLength = 8.0f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private bool moveDirection;


    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (boss == null)
        {
            boss = GetComponentInParent<Boss>();
        }
        startPosition = transform.localPosition;
    }

    private void Start()
    {
        StartCoroutine(LengthMeasureCo());
    }

    private void FixedUpdate()
    {
        Move(); 
    }

    

    private void Move()
    {
        if(moveDirection)
        {
            rb.linearVelocity = new Vector2(movingSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-movingSpeed, 0);
        }
    }

    IEnumerator LengthMeasureCo()
    {
        while (true)
        {
            if (Mathf.Abs(transform.localPosition.x - startPosition.x) >= movingLength)
            {
                moveDirection = !moveDirection;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }
}
