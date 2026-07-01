using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkullManIdleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float waitTimeMin = 2f;
    [SerializeField] private float waitTimeMax = 5f;
    [SerializeField] private float moveTimeMin = 2f;
    [SerializeField] private float moveTimeMax = 5f;
    [SerializeField] private SkullMan skullMan;

    [SerializeField] private Transform groundBase;

    [SerializeField] DogAnimation dogAnimation;

    private Rigidbody2D rb;
    private Rigidbody2D GBrb;
    private float jumpCoolTime;

    private bool verti;
    private int dir = 0;
    private float moveTime = 0;
    private float waitTime = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        skullMan = GetComponent<SkullMan>();
        GBrb = groundBase.GetComponent<Rigidbody2D>();
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
                rb.linearVelocity = Vector2.zero;
                GBrb.linearVelocity = Vector2.zero;
                ChangeDirection();
                dogAnimation.SetIdle();
                MoveTimeSet();
                WaitTimeSet();

                yield return new WaitForSeconds(waitTime);
            }
            // 이동
            else 
            {
                moveTime -= Time.deltaTime;
                
                IdleMove();
                IdleFly();

                if(GBrb.linearVelocity ==  Vector2.zero)
                {
                    dogAnimation.SetIdle();
                }
                else
                {
                    dogAnimation.SetWalk();
                }

                

                yield return null;
            }
        }
    }

    private void IdleMove()
    {
        // 우
        if (dir == 0)
        {
            
            GBrb.linearVelocity = new Vector2(moveSpeed, 0);
            rb.linearVelocity = new Vector2(GBrb.linearVelocity.x, rb.linearVelocity.y);
            if(Mathf.Abs(GBrb.position.x - transform.position.x) > 0.1f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        // 좌
        else
        {
            
            GBrb.linearVelocity = new Vector2(-moveSpeed, 0);
            rb.linearVelocity = new Vector2(GBrb.linearVelocity.x, rb.linearVelocity.y);

            if (Mathf.Abs(GBrb.position.x - transform.position.x) > 0.1f)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }

    private void IdleFly()
    {
        
        if (transform.position.y < groundBase.position.y + 2.0f)
        {
            verti = true;
        }
        else if(transform.position.y > groundBase.position.y + 4.0f)
        {
            verti = false;
        }
        VerticalMove(verti);
            
    }

    private void VerticalMove(bool up)
    {
        if(up)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -moveSpeed);
        }
    }

    // 방향설정
    private void ChangeDirection()
    {
        dir = Random.Range(0, 2);
        if(dir == 0)
        {
            skullMan.SetDirection(true);
        }
        else skullMan.SetDirection(false);
    }

    // 이동시간
    private void MoveTimeSet()
    {
        moveTime = Random.Range(moveTimeMin, moveTimeMax);
    }

    // 정지시간
    private void WaitTimeSet()
    {
        waitTime = Random.Range(waitTimeMin, waitTimeMax);
    }

}
