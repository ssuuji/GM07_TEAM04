using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement { get; private set; } = Vector2.zero;
    public static bool IsJump { get; private set; }
    public static bool IsDash { get; private set; }
    public static bool IsInteract { get; private set; }
    public static bool IsBasicAttack { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction interactAction;
    private InputAction basicAttackAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Sprint");
        interactAction = InputSystem.actions.FindAction("Interact");
        basicAttackAction = InputSystem.actions.FindAction("BasicAttack");
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();              //이동 : 방향키 ← → ↑ ↓ 
        IsJump = jumpAction.WasPressedThisFrame();               //점프 : 스페이스바
        IsDash = dashAction.WasPressedThisFrame();               //대쉬 : 왼쪽 쉬프트
        IsInteract = interactAction.WasPressedThisFrame();       //상호작용 : F
        IsBasicAttack = basicAttackAction.WasPressedThisFrame(); //기본공격 : Z
    }
}
