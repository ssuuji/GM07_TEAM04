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

        if (InputManager.IsJump)
        {
            playerJump.Jump();
        }

        if (InputManager.IsDash)
        {
            playerDash.Dash(playerMovement.CheckDirValue);
        }

        if (InputManager.IsBasicAttack)
        {
            playerAttack.Attack();
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