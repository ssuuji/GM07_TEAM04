using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement { get; private set; } = Vector2.zero;
    public static bool IsJump { get; private set; }
    public static bool IsDash { get; private set; }
    public static bool IsInteract { get; private set; }
    public static bool IsBasicAttack { get; private set; }
    public static bool IsAreaAttack { get; private set; }
    public static bool IsBuff { get; private set; }
    public static bool IsInvin { get; private set; }
    public static bool IsStatus { get; private set; }
    public static bool IsEsc { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction interactAction;
    private InputAction basicAttackAction;
    private InputAction areaAttackAction;
    private InputAction buffAction;
    private InputAction invinAction;
    private InputAction statusAction;
    private InputAction escAction;

    // Inventory Part
    // Add I Input
    public static bool IsOpenInventory { get; private set; } = false;
    private InputAction openInventoryAction;
    // Add NumberKey 1~3 Input
    public static bool IsQuickSlot1 { get; private set; }
    public static bool IsQuickSlot2 { get; private set; }
    public static bool IsQuickSlot3 { get; private set; }
    private InputAction quickSlot1;
    private InputAction quickSlot2;
    private InputAction quickSlot3;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Sprint");
        interactAction = InputSystem.actions.FindAction("Interact");
        basicAttackAction = InputSystem.actions.FindAction("BasicAttack");
        areaAttackAction = InputSystem.actions.FindAction("AreaAttack");
        buffAction = InputSystem.actions.FindAction("Buff");
        invinAction = InputSystem.actions.FindAction("Invin");
        statusAction = InputSystem.actions.FindAction("Status");
        escAction = InputSystem.actions.FindAction("Esc");
        // Inventory Part
        // Add I Input
        if (openInventoryAction == null)
        {
            openInventoryAction = InputSystem.actions.FindAction("Inventory");
        }
        // Add NumberKey 1~3 Input
        if (quickSlot1 == null)
        {
            quickSlot1 = InputSystem.actions.FindAction("QuickSlot1");
        }
        if (quickSlot2 == null)
        {
            quickSlot2 = InputSystem.actions.FindAction("QuickSlot2");
        }
        if (quickSlot3 == null)
        {
            quickSlot3 = InputSystem.actions.FindAction("QuickSlot3");
        }
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();              //이동 : 방향키 ← → ↑ ↓ 
        IsJump = jumpAction.WasPressedThisFrame();               //점프 : 스페이스바
        IsDash = dashAction.WasPressedThisFrame();               //대쉬 : 왼쪽 쉬프트
        IsInteract = interactAction.WasPressedThisFrame();       //상호작용 : F
        IsBasicAttack = basicAttackAction.WasPressedThisFrame(); //기본공격 : Z
        IsAreaAttack = areaAttackAction.WasPressedThisFrame();   //범위공격 : X
        IsBuff = buffAction.WasPressedThisFrame();               //공격버프 : C
        IsInvin = invinAction.WasPressedThisFrame();             //무적기   : V
        IsStatus = statusAction.WasPressedThisFrame();           //스탯창   : Tab
        IsEsc = escAction.WasPressedThisFrame();                 //닫기     : Esc
        // Inventory Part
        // Add I Input
        IsOpenInventory = openInventoryAction.WasPressedThisFrame();
        // Add NumberKey 1~3 Input
        IsQuickSlot1 = quickSlot1.WasPressedThisFrame();
        IsQuickSlot2 = quickSlot2.WasPressedThisFrame();
        IsQuickSlot3 = quickSlot3.WasPressedThisFrame();
    }
}
