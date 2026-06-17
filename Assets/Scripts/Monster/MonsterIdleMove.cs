using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterIdleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float waitTimeMin = 2f;
    [SerializeField] private float waitTimeMax = 5f;
    [SerializeField] private float moveTimeMin = 2f;
    [SerializeField] private float moveTimeMax = 5f;
    [SerializeField] private GroundChecker groundChecker;
    private Rigidbody2D rb;
    private float jumpCoolTime;

    private int dir = 0;
    private float moveTime = 0;
    private float waitTime = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(MonsterMoveCo());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator MonsterMoveCo()
    {
        while (true)
        {   // 정지
            if (moveTime <= 0)
            {
                ChangeDirection();
                MoveTimeSet();
                WaitTimeSet();

                yield return new WaitForSeconds(waitTime);
            }
            // 이동
            else 
            {
                moveTime -= Time.deltaTime;

                IdleMove();
                IdleJump();

                yield return null;
            }
        }
    }

    private void IdleMove()
    {
        // 우
        if(dir == 0)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }

        // 좌
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }
    }

    private void IdleJump()
    {
        jumpCoolTime -= Time.deltaTime;
        if (jumpCoolTime <= 0 && groundChecker.IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpCoolTime = 2f;
        }
    }

    

    private void ChangeDirection()
    {
        dir = Random.Range(0, 2);
    }

    private void MoveTimeSet()
    {
        moveTime = Random.Range(moveTimeMin, moveTimeMax);
    }

    private void WaitTimeSet()
    {
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
    }

}
