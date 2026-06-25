using UnityEngine;

public class DogAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;


    private bool action;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetIdle()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Run", false);
    }
    public void SetWalk()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);
    }

    public void SetJump()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);
        animator.SetTrigger("Fire");
    }

    public void Attack()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetTrigger("Attack");
    }
    //
    public void Hit()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetTrigger("Hit");
    }

    public void Die()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Die", true);
    }

   
}
