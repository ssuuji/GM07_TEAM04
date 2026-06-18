using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector3 originScale;
    private float checkDir = 1f;

    public float CheckDirValue => checkDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
    }

    public void Move()
    {
        rb.linearVelocity = new Vector2(InputManager.Movement.x * moveSpeed, rb.linearVelocity.y);
    }

    public void CheckDir()
    {
        if (InputManager.Movement.x > 0)
        {
            checkDir = 1f;
        }
        else if (InputManager.Movement.x < 0)
        {
            checkDir = -1f;
        }

        transform.localScale = new Vector3(Mathf.Abs(originScale.x) * checkDir, originScale.y, originScale.z);
    }
}