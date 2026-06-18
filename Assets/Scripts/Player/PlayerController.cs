using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private PlayerDash playerDash;
    private PlayerAttack playerAttack;
    private PlayerInteraction playerInteraction;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        playerJump.CheckGround();
        playerMovement.CheckDir();

        //점프
        if (InputManager.IsJump)
        {
            playerJump.Jump();
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
        if (InputManager.IsInteract)
        {
            playerInteraction.Interact();
        }
    }

    private void FixedUpdate()
    {
        if (playerDash.IsDash) return;

        playerMovement.Move();
    }
}