using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    public const float walkSpeed = 0.5f;
    public const float runSpeed = 1.0f;
    public const float sprintSpeed = 1.5f;
    private PlayerMovement playerMovement;
    private PlayerInputHandler inputHandler;

    [Range(0f, 1f)]
    public float freeSmooth = 0.2f, strafeSmooth = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.Fixed;
        playerMovement = GetComponent<PlayerMovement>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (animator == null || !animator.enabled) return;

        animator.SetBool(AnimatorParameters.IsStrafing, playerMovement.IsStrafing);
        animator.SetBool(AnimatorParameters.IsSprinting, playerMovement.IsSprinting);
        animator.SetBool(AnimatorParameters.IsGrounded, playerMovement.IsGrounded);
        if (playerMovement.IsJumping)
        {
            if (inputHandler.MoveInput.magnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", 0.2f);
        }
        // animator.SetFloat(AnimatorParameters.GroundDistance, playerMovement.GroundDistance);

        Vector3 relativeInput = transform.InverseTransformDirection(playerMovement.MoveDirection);
        Vector2 newInput = new Vector2(relativeInput.z, relativeInput.x);
        float inputMagnitude = GetInputMagnitude(newInput.magnitude);

        if (playerMovement.IsStrafing)
        {
            animator.SetFloat(AnimatorParameters.InputHorizontal, newInput.y, strafeSmooth, Time.deltaTime);
            animator.SetFloat(AnimatorParameters.InputVertical, newInput.x, freeSmooth, Time.deltaTime);
            animator.SetFloat(AnimatorParameters.InputMagnitude, inputMagnitude, strafeSmooth, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(AnimatorParameters.InputVertical, newInput.x, freeSmooth, Time.deltaTime);
            animator.SetFloat(AnimatorParameters.InputMagnitude, inputMagnitude, freeSmooth, Time.deltaTime);
        }

    }

    private float GetInputMagnitude(float vl)
    {
        switch (playerMovement.MovementMode)
        {
            case MovementMode.Walking:
                return Mathf.Clamp(vl, 0, walkSpeed);
            case MovementMode.Running:
                return Mathf.Clamp(vl, 0, runSpeed);
            case MovementMode.Sprinting:
                return Mathf.Clamp(vl, 0, sprintSpeed);
            default:
                return 0f;
        }
    }


    private static class AnimatorParameters
    {
        public static readonly int InputHorizontal = Animator.StringToHash("InputHorizontal");
        public static int InputVertical = Animator.StringToHash("InputVertical");
        public static int InputMagnitude = Animator.StringToHash("InputMagnitude");
        public static int IsGrounded = Animator.StringToHash("IsGrounded");
        public static int IsStrafing = Animator.StringToHash("IsStrafing");
        public static int IsSprinting = Animator.StringToHash("IsSprinting");
        public static int GroundDistance = Animator.StringToHash("GroundDistance");
    }
}
