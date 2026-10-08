using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private MovementSettings movementSt;

    [Header("Airborne Settings")]
    [SerializeField] private AirborneSettings airborneSt;

    [Header("Ground Settings")]
    [SerializeField] private PlayerGroundSettings groundSt;

    private CharacterController characterController;
    private PlayerInputHandler inputHandler;

    public MovementMode MovementMode { get; private set; }
    private MovementMode baseMovementMode;
    public bool IsJumping { get; private set; }
    public bool IsStrafing { get; private set; }

    public bool IsSprinting { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    // used to know the direction you're moving
    private bool sprintToogled;
    private float moveSpeed;                        // set the current moveSpeed for the MoveCharacter method
    private float verticalVelocity;
    private float heightReached;                    // max height that character reached in air
    private float jumpCounter;                      // used to count the routine to reset the jump
    // private float groundDistance;
    private bool lockMovement = false;              // lock the movement of the controller (not the animation)
    private bool lockRotation = false;              // lock the rotation of the controller (not the animation)
    private Transform rotateTarget;                 // used as a generic reference for the camera.transform

    // private Vector3 input;
    private Vector3 inputSmooth;                    // used to smooth the input
    private Vector3 inputSmoothVelocity;

    public bool IsGrounded => characterController.isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        baseMovementMode = movementSt.mode == MovementMode.Sprinting
            ? MovementMode.Running
            : movementSt.mode;
        MovementMode = baseMovementMode;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     MovementMode = movementSt.mode;
    // }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        ApplyGravity();
        UpdateInputSmooth();
        UpdateMoveDirection();
        UpdateSpeed();
        UpdateJumpAndGravity();
        ApplyMovement();
        UpdateRotation();
    }

    // private void UpdateGroundedState()
    private void ApplyGravity()
    {
        if (IsGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            IsJumping = false;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

            // Giới hạn tốc độ rơi
            verticalVelocity = Mathf.Max(
                verticalVelocity,
                airborneSt.limitFallVelocity
            );
        }
    }


    private bool ShouldUseStrafeSpeed()
    {
        switch (movementSt.locomotionType)
        {
            case LocomotionType.OnlyStrafe:
                return true;

            case LocomotionType.FreeWithStrafe:
                return IsStrafing;

            case LocomotionType.OnlyFree:
            default:
                return false;
        }
    }

    public float GetTargetSpeed()
    {
        MovementSpeed speed = ShouldUseStrafeSpeed() ? movementSt.strafeSpeed : movementSt.freeSpeed;

        switch (MovementMode)
        {
            case MovementMode.Walking:
                return speed.walkSpeed;

            case MovementMode.Running:
                return speed.runSpeed;

            case MovementMode.Sprinting:
                return speed.sprintSpeed;

            default:
                return moveSpeed;
        }
    }


    private void UpdateInputSmooth()
    {
        Vector3 targetInput = lockMovement ? Vector3.zero : inputHandler.MoveInput;
        bool useStrafeSpeed = ShouldUseStrafeSpeed();
        float smooth = IsGrounded ? (useStrafeSpeed ? movementSt.strafeSpeed.movementSmooth : movementSt.freeSpeed.movementSmooth) : airborneSt.airSmooth;
        float smoothTime = 1f / Mathf.Max(smooth, 0.01f);

        inputSmooth = Vector3.SmoothDamp(
            inputSmooth,
            targetInput,
            ref inputSmoothVelocity,
            smoothTime,
            Mathf.Infinity,
            Time.deltaTime
        );

        if (inputSmooth.sqrMagnitude > 1f)
            inputSmooth.Normalize();
    }

    private void UpdateMoveDirection()
    {
        if (cameraTransform == null)
        {
            MoveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        MoveDirection = cameraForward * inputSmooth.z + cameraRight * inputSmooth.x;

        if (MoveDirection.sqrMagnitude > 1f)
            MoveDirection.Normalize();
    }

    private void UpdateSpeed()
    {
        if (inputHandler.ConsumeWalkTogglePressed())
        {
            baseMovementMode = baseMovementMode == MovementMode.Walking
                ? MovementMode.Running
                : MovementMode.Walking;
        }
        if (!movementSt.useContinuousSprint && inputHandler.ConsumeSprintPressed())
            sprintToogled = !sprintToogled;

        bool hasMovementInput = inputSmooth.sqrMagnitude > 0.001f && !lockMovement;
        if (!hasMovementInput)
            IsSprinting = false;

        bool sprintRequested = movementSt.useContinuousSprint
            ? inputHandler.SprintHeld
            : sprintToogled;

        IsSprinting = hasMovementInput && sprintRequested;

        MovementMode targetMode = IsSprinting ? MovementMode.Sprinting : baseMovementMode;

        MovementMode = targetMode;
        float targetSpeed = GetTargetSpeed(

        );

        float rate = IsGrounded
            ? (ShouldUseStrafeSpeed() ? movementSt.strafeSpeed.movementSmooth : movementSt.freeSpeed.movementSmooth)
            : airborneSt.airSmooth;

        moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, rate * Time.deltaTime);

    }

    private void ApplyMovement()
    {
        float mvSpeed = IsGrounded ? moveSpeed : airborneSt.airSpeed;
        Vector3 horizontalVelocity = MoveDirection * mvSpeed;
        Vector3 velecity = horizontalVelocity;
        velecity.y = verticalVelocity;
        characterController.Move(velecity * Time.deltaTime);
    }

    private void UpdateJumpAndGravity()
    {
        if (inputHandler.ConsumeJumpPressed() && IsGrounded && !lockMovement)
        {
            verticalVelocity = Mathf.Sqrt(
                airborneSt.jumpHeight * -2f * airborneSt.extraGravity);
            jumpCounter = airborneSt.jumpTimer;
            IsJumping = true;
        }

        if (IsJumping)
        {
            jumpCounter -= Time.deltaTime;
            if (jumpCounter <= 0f)
                IsJumping = false;
        }

        if (!IsGrounded || verticalVelocity > 0f)
        {
            verticalVelocity += airborneSt.extraGravity * Time.deltaTime;
            verticalVelocity = Mathf.Max(
                verticalVelocity,
                airborneSt.limitFallVelocity);
        }
    }

    private void UpdateRotation()
    {
        if (lockRotation || MoveDirection.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            (ShouldUseStrafeSpeed() ? movementSt.strafeSpeed.rotationSpeed : movementSt.freeSpeed.rotationSpeed) * Time.deltaTime

        );
    }
}

public enum MovementMode
{
    Walking,
    Running,
    Sprinting
}

public enum LocomotionType
{
    FreeWithStrafe,
    OnlyStrafe,
    OnlyFree
}

[System.Serializable]
public class MovementSpeed
{
    [Range(0f, 20f)]
    public float movementSmooth = 6f;

    // [Range(0f, 1f)]
    // public float animationSmooth = 0.2f;

    [Tooltip("Rotation speed of the character")]
    [Range(0f, 1080f)]
    public float rotationSpeed = 360f;

    [Tooltip("Speed to Walk or extra speed if you're using RootMotion")]
    public float walkSpeed = 2f;

    [Tooltip("Speed to Run or extra speed if you're using RootMotion")]
    public float runSpeed = 4f;

    [Tooltip("Speed to Sprint or extra speed if you're using RootMotion")]
    public float sprintSpeed = 6f;
}

[System.Serializable]
public class MovementSettings
{
    [Tooltip("Turn off if you have 'in place' animations and use this values above to move the character, or use with root motion as extra speed")]
    public bool useRootMotion = false;

    [Tooltip("Use this to rotate the character using the World axis, or false to use the camera axis - CHECK for Isometric Camera")]
    public bool rotateByWorld = false;

    [Tooltip("Rotate with the Camera forward when standing idle")]
    public bool rotateWithCamera = false;

    [Tooltip("Check this to use sprint on press button to your Character run until the stamina finish or movement stop\nIf uncheck your Character will sprint as long as the SprintInput is pressed or the stamina finishes")]
    public bool useContinuousSprint = true;

    public LocomotionType locomotionType = LocomotionType.FreeWithStrafe;

    [Tooltip("Default movement mode or old movement mode")]
    public MovementMode mode = MovementMode.Running;

    [Tooltip("Check this to sprint alawys in free movement")]
    public bool sprintOnlyFree = true;

    public MovementSpeed freeSpeed, strafeSpeed;
}

[System.Serializable]
public class AirborneSettings
{
    [Tooltip("Rotate or not while airborne")]
    public bool jumpAndRotate = true;

    [Tooltip("How much time the charater will be jumping")]
    public float jumpTimer = 0.3f;

    [Tooltip("Add Extra jump height, if you want to jump only with RootMotion leave the value with 0.")]
    public float jumpHeight = 4f;

    [Tooltip("Speed that the character will move while airborne")]
    public float airSpeed = 5f;

    [Tooltip("Smoothness of the direction while airborne")]
    public float airSmooth = 6f;

    [Tooltip("Apply extra gravity when the character is not grounded")]
    public float extraGravity = -10f;

    [HideInInspector]
    public float limitFallVelocity = -15f;
}

[System.Serializable]
public class PlayerGroundSettings
{
    [Tooltip("Layers that the character can walk on")]
    public LayerMask groundLayer = 1 << 0;

    [Tooltip("Distance to became not grounded")]
    public float groundMinDistance = 0.25f;
    public float groundMaxDistance = 0.5f;

    [Tooltip("Max angle to walk")]
    [Range(30, 80)] public float slopeLimit = 75f;
}

