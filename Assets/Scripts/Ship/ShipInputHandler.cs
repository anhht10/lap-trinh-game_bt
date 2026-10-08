using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ShipInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    public Vector3 MoveInput { get; private set; }
    public Vector3 LookInput { get; private set; }
    public float RollInput { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool AttackHold { get; private set; }

    private bool _sprintPressed;

    public bool TPCameraTogger { get; private set; } = true;

    public bool MouseLocked { get; private set; } = true;

    public event Action<int> OnSkillEvent;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Look"].performed += OnLook;
        _playerInput.actions["Look"].canceled += OnLook;
        _playerInput.actions["Roll"].performed += OnRoll;
        _playerInput.actions["Roll"].canceled += OnRoll;
        _playerInput.actions["Attack"].performed += OnAttack;
        _playerInput.actions["Attack"].canceled += OnAttack;
        _playerInput.actions["Sprint"].performed += OnSprint;
        _playerInput.actions["Sprint"].canceled += OnSprint;
        _playerInput.actions["LockMouse"].performed += OnLockMouse;
        _playerInput.actions["LockMouse"].canceled += OnLockMouse;

        _playerInput.actions["SwitchCamera"].performed += OnSwitchCamera;
        // _playerInput.actions["SwitchCamera"].canceled += OnSwitchCamera;

        _playerInput.actions["Skills"].performed += OnSkillInput;
        // _playerInput.actions["Number2"].performed += context => NumberInput = 2;
        // _playerInput.actions["Number3"].performed += context => NumberInput = 3;
        // _playerInput.actions["Number4"].performed += context => NumberInput = 4;
        // _playerInput.actions["Number5"].performed += context => NumberInput = 5;    
    }

    private void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Look"].performed -= OnLook;
        _playerInput.actions["Look"].canceled -= OnLook;
        _playerInput.actions["Roll"].performed -= OnRoll;
        _playerInput.actions["Roll"].canceled -= OnRoll;
        _playerInput.actions["Attack"].performed -= OnAttack;
        _playerInput.actions["Attack"].canceled -= OnAttack;
        _playerInput.actions["Sprint"].performed -= OnSprint;
        _playerInput.actions["Sprint"].canceled -= OnSprint;
        _playerInput.actions["LockMouse"].performed -= OnLockMouse;
        _playerInput.actions["LockMouse"].canceled -= OnLockMouse;

        _playerInput.actions["SwitchCamera"].performed -= OnSwitchCamera;
        // _playerInput.actions["SwitchCamera"].canceled -= OnSwitchCamera;

        _playerInput.actions["Skills"].performed -= OnSkillInput;
    }
    // private float _moveStartTime;
    // private bool _isMoving;
    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        RollInput = context.ReadValue<float>();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        AttackHold = context.ReadValueAsButton();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            _sprintPressed = true;

        SprintHeld = context.ReadValueAsButton();
    }

    public bool ConsumeSprintPressed()
    {
        if (!_sprintPressed)
            return false;
        _sprintPressed = false;
        return true;
    }

    private void OnLockMouse(InputAction.CallbackContext context)
    {
        MouseLocked = !context.ReadValueAsButton();
    }

    private void OnSwitchCamera(InputAction.CallbackContext context)
    {
        TPCameraTogger = !TPCameraTogger;
    }


    private void OnSkillInput(InputAction.CallbackContext context)
    {
        int skillIndex = (int)context.ReadValue<float>();
        OnSkillEvent?.Invoke(skillIndex);
    }
}
