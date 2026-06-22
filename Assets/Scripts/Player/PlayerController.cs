using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private PlayerDash playerDash;
    private PlayerAttack playerAttack;
    private PlayerInteraction playerInteraction;
    private PlayerWall playerWall;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerWall = GetComponent<PlayerWall>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        playerJump.CheckGround();    //바닥 체크
        playerWall.CheckWall();      //벽 체크
        playerWall.UpdateWallJump(); //벽점프상태 체크
        playerMovement.CheckDir();   //방향 체크

        //점프
        if (InputManager.IsJump)
        {
            //벽에 닿아있고 공중에 있다면 벽점프
            if (playerWall.IsWall && !playerJump.IsGround)
            {
                playerWall.WallJump();
            }
            //일반 점프
            else
            {
                playerJump.Jump();
            }
        }
        //대쉬
        if (InputManager.IsDash)
        {
            playerDash.Dash(playerMovement.CheckDirValue);
        }
        //기본공격
        if (InputManager.IsBasicAttack)
        {
            playerAttack.Attack();
        }
        //범위공격
        if (InputManager.IsAreaAttack)
        {
            playerAttack.AreaAttack();
        }
        //공격버프
        if (InputManager.IsBuff)
        {
            playerAttack.Buff();
        }
        //무적기
        if (InputManager.IsInvin)
        {
            playerAttack.Invin();
        }
        //상호작용
        if (InputManager.IsInteract)
        {
            playerInteraction.Interact();
        }
    }

    private void FixedUpdate()
    {
        if (playerDash.IsDash) return;        //대쉬 중에는 Move로 속도를 덮지 않도록 
        if (playerWall.IsWallJump) return;    //벽점프 직후에는 X
        if (playerHealth.IsKnockBack) return; //넉백 중에는 X

        //이동
        playerMovement.Move();

        //벽슬라이드
        playerWall.WallSlide();
    }
}