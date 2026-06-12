using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement { get; private set; } = Vector2.zero;
    public static bool IsJump { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();
        IsJump = jumpAction.WasPressedThisFrame();
    }
}
