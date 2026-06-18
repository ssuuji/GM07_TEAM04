using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded()
    {
        return Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer);
    }
}
