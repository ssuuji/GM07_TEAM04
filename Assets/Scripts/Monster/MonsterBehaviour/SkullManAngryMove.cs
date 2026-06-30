using UnityEngine;

public class SkullManAngryMove : MonoBehaviour
{
    [SerializeField] SkullMan skullMan;
    [SerializeField] SkullManAttack attack;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float aroundRange = 1f;
    [SerializeField] private float attackRange = 3f;

    [SerializeField] DogAnimation dogAnimation;

    [SerializeField] private Transform groundBase;
    [SerializeField] private Transform player;

    

    private Rigidbody2D rb;
    private bool dir;
    private bool moveVer;
    private bool moveHor;

    private Vector2 moveCenter;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dogAnimation = GetComponent<DogAnimation>();
        player = GameObject.FindWithTag("Player").transform;
        skullMan = GetComponent<SkullMan>();
    }

    


    private void Update()
    {
        FindDirection();
        SkullManInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // 플레이어 상대 방향찾기
    private void FindDirection()
    {
        dir = (player.transform.position.x >= transform.position.x) ? true : false;
        skullMan.SetDirection(dir);
    }

    // 추적
    private void Move()
    {
        dogAnimation.Attack();
        BaseMove();
        
        VertiMove(moveVer);
        HorizMove(moveHor);
    }

    private void SkullManInput()
    {
        moveCenter = groundBase.position + Vector3.up * 3;
        if(transform.position.x < moveCenter.x - 2)
        {
            moveHor = true;
        }
        else if (transform.position.x > moveCenter.x + 2)
        {
            moveHor = false;
        }

        if (Mathf.Abs(transform.position.y - moveCenter.y) > 0.5f)
        {
            if(transform.position.y < moveCenter.y)
            {
                moveVer = true;
            }
            else if (transform.position.y > moveCenter.y)
            {
                moveVer = false;
            }
        }

        if(InRange())
        {
            attack.enabled = true;
        }
        else
        {
            attack.enabled = false;
        }
    }

    private void VertiMove(bool dir)
    {
        if (dir)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.3f);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.3f);
        }
    }

    private void HorizMove(bool dir)
    {
        if (dir)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }
    }


    private void BaseMove()
    {
        groundBase.position = Vector3.Lerp(
            groundBase.position,
            player.position,
            moveSpeed * Time.deltaTime);
    }

    private bool InRange()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) < attackRange)
        {
            return true;
        }
        else return false;
    }

}
