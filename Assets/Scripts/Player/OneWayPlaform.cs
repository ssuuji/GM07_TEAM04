using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private bool isPlayer;

    private Collider2D platformCollider;
    private Collider2D playerCollider;
    private bool isDropping;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        platformCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(DisableCollisionRoutine());
        }
    }
    private IEnumerator DisableCollisionRoutine()
    {
        isDropping = true;
        Physics2D.IgnoreCollision(platformCollider, playerCollider, true); //플레이어와 발판사이 충돌만 무시
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false); //충돌 적용
        isDropping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayer = true;
            playerCollider = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayer = false;
            if (!isDropping)
            {
                playerCollider = null;
            }
        }
    }
}