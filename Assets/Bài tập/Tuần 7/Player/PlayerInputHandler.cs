using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    public Vector3 MoveInput { get; private set; }
    // public Vector3 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool WalkToggledPressed { get; private set; }
    public bool CursorUnLockHeld { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        if (playerInput == null || playerInput.actions == null)
            return;

        InputActionMap playerMap = playerInput.actions.FindActionMap("Player", true);

        playerMap.FindAction("Move", true).performed += OnMove;
        playerMap.FindAction("Move", true).canceled += OnMove;
        // playerMap.FindAction("Look", true).performed += OnLook;
        // playerMap.FindAction("Look", true).canceled += OnLook;
        playerMap.FindAction("Jump", true).performed += OnJump;
        playerMap.FindAction("Sprint", true).performed += OnSprint;
        playerMap.FindAction("Sprint", true).canceled += OnSprint;
        playerMap.FindAction("CursorUnlock", true).performed += OnCursorUnlock;
        playerMap.FindAction("CursorUnlock", true).canceled += OnCursorUnlock;
        // playerMap.FindAction("WalkToggle", true).performed += OnWalkToggle;

    }

    private void OnDisable()
    {
        if (playerInput == null || playerInput.actions == null)
            return;

        InputActionMap playerMap = playerInput.actions.FindActionMap("Player", false);
        if (playerMap == null)
            return;

        playerMap.FindAction("Move", false).performed -= OnMove;
        playerMap.FindAction("Move", false).canceled -= OnMove;
        // playerMap.FindAction("Look", false).performed -= OnLook;
        // playerMap.FindAction("Look", false).canceled -= OnLook;
        playerMap.FindAction("Jump", false).performed -= OnJump;
        playerMap.FindAction("Sprint", false).performed -= OnSprint;
        playerMap.FindAction("Sprint", false).canceled -= OnSprint;
        playerMap.FindAction("CursorUnlock", false).performed -= OnCursorUnlock;
        playerMap.FindAction("CursorUnlock", false).canceled -= OnCursorUnlock;
        // playerMap.FindAction("WalkToggle", false).performed -= OnWalkToggle;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove called");
        Vector2 input = context.ReadValue<Vector2>();
        MoveInput = new Vector3(input.x, 0f, input.y);
    }

    // public void OnLook(InputAction.CallbackContext context)
    // {
    //     LookInput = context.ReadValue<Vector2>();
    //     // Debug.Log($"OnLook called. LookInput: {LookInput}");
    // }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpPressed = context.ReadValueAsButton();
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            SprintPressed = true;
        SprintHeld = context.ReadValueAsButton();
    }

    private void OnWalkToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
            WalkToggledPressed = true;
    }

    private void OnCursorUnlock(InputAction.CallbackContext context)
    {
        CursorUnLockHeld = context.ReadValueAsButton();
    }

    public bool ConsumeJumpPressed()
    {
        if (!JumpPressed)
            return false;

        JumpPressed = false;
        return true;
    }


    public bool ConsumeSprintPressed()
    {
        if (!SprintPressed)
            return false;

        SprintPressed = false;
        return true;
    }

    public bool ConsumeWalkTogglePressed()
    {
        if (!WalkToggledPressed)
            return false;

        WalkToggledPressed = false;
        return true;
    }
}
