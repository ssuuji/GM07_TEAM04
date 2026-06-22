using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DroppingPoint : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float movingLength = 8.0f;

    private Rigidbody2D rb;
    private Transform startPosition;
    private bool moveDirection;


    private void Awake()
    {
        if (boss == null)
        {
            boss = GetComponent<Boss>();
        }

        startPosition.position = transform.position;

        UpdateDirection();
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

    private void LengthMeasure()
    {
        if(Mathf.Abs(transform.position.x - startPosition.position.x) > movingLength)
        {
            moveDirection = !moveDirection;
        }
    }
}
