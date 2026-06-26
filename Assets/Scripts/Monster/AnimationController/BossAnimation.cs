using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        Debug.Log("Animator = "  +animator);
        
    }


    public void StartMove()
    {
        animator.SetBool("IsMove", true);
    }

    public void EndMove()
    {
        animator.SetBool("IsMove", false);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void Return()
    {
        animator.SetTrigger("Return");
    }
}
