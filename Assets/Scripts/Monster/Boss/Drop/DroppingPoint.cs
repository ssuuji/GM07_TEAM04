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
    private float timer;


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
        UpdateDirection();
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

    private void UpdateDirection()
    {
        moveDirection = boss.Direction;

        if (moveDirection)
        {
            transform.localPosition = new Vector3(1, 6, 0);
        }

        else
        {
            transform.localPosition = new Vector3(-1, 6, 0);
        }
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
