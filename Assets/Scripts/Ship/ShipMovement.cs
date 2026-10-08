
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ShipInputHandler))]
public class ShipMovement : MonoBehaviour
{
    [SerializeField] private ShipMovementSettings _movementSettings = new();

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;
    private ShipInputHandler _input;

    private Vector2 _smoothLookInput;
    private Vector2 _lookInputVelocity;
    private Vector2 _smoothMoveInput;
    private Vector2 _moveInputVelocity;
    private float _smoothRollInput;
    private float _rollInputVelocity;
    private Vector3 _movementDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<ShipInputHandler>();

        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotate();
    }

    private void UpdateSmoothMove()
    {
        Vector2 targetMoveInput = Vector2.ClampMagnitude(_input.MoveInput, 1f);
        _smoothMoveInput = Vector2.SmoothDamp(
            _smoothMoveInput,
            targetMoveInput,
            ref _moveInputVelocity,
            _movementSettings.MoveSmoothing,
            Mathf.Infinity,
            Time.fixedDeltaTime);
    }

    private void UpdateMoveDirection()
    {
        Vector3 direction;
        if (_input.TPCameraTogger && _cameraTransform != null)
        {
            Vector3 cameraForward = _cameraTransform.forward;

            Vector3 cameraRight = Vector3.ProjectOnPlane(
                _cameraTransform.right,
                Vector3.up);

            if (cameraForward.sqrMagnitude > 0.001f)
                cameraForward.Normalize();
            if (cameraRight.sqrMagnitude > 0.001f)
                cameraRight.Normalize();

            direction = cameraRight * _smoothMoveInput.x + cameraForward * _smoothMoveInput.y;
        }
        else
        {
            direction = transform.right * _smoothMoveInput.x +
                transform.forward * _smoothMoveInput.y;
        }

        _movementDirection = direction.sqrMagnitude > 0.001f
            ? direction.normalized
            : Vector3.zero;
    }

    private void ApplyMovement()
    {
        UpdateSmoothMove();
        UpdateMoveDirection();

        float currentSpeed = _input.SprintHeld
            ? _movementSettings.MaxSpeed * _movementSettings.SprintMultiplier
            : _movementSettings.MaxSpeed;


        if (_movementDirection.sqrMagnitude > 0.001f)
        {
            Vector3 targetVelocity =
                _movementDirection * currentSpeed;

            _rigidbody.linearVelocity = Vector3.MoveTowards(
                _rigidbody.linearVelocity,
                targetVelocity,
                _movementSettings.Acceleration *
                Time.fixedDeltaTime
            );
        }
        else
        {
            // Tàu giảm tốc từ từ thay vì dừng ngay.
            _rigidbody.linearVelocity = Vector3.MoveTowards(
                _rigidbody.linearVelocity,
                Vector3.zero,
                _movementSettings.Deceleration *
                Time.fixedDeltaTime
            );
        }

        float velocityMagnitude = _rigidbody.linearVelocity.magnitude;

        if (velocityMagnitude > currentSpeed)
        {
            _rigidbody.linearVelocity =
                _rigidbody.linearVelocity.normalized *
                currentSpeed;
        }
        // Debug.Log(
        //     $"After = {_rigidbody.linearVelocity.magnitude:F2}"
        // );
    }

    private void UpdateSmoothLook()
    {
        if (!_input.MouseLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _smoothLookInput = Vector2.zero;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector2 targetLookInput = Vector2.ClampMagnitude(_input.LookInput, 1f);
        _smoothLookInput = Vector2.SmoothDamp(
            _smoothLookInput,
            targetLookInput,
            ref _lookInputVelocity,
            _movementSettings.LookSmoothing,
            Mathf.Infinity,
            Time.fixedDeltaTime);
    }
    private void UpdateSmoothRoll()
    {
        float targetRollInput = Mathf.Clamp(_input.RollInput, -1f, 1f);
        _smoothRollInput = Mathf.SmoothDamp(
            _smoothRollInput,
            targetRollInput,
            ref _rollInputVelocity,
            _movementSettings.RollSmoothing,
            Mathf.Infinity,
            Time.fixedDeltaTime);
    }

    private void ApplyRotate()
    {
        UpdateSmoothRoll();
        if (_input.TPCameraTogger)
        {
            RotateTowardsMovement();
            return;
        }

        UpdateSmoothLook();

        Vector3 rotationInput = new Vector3(
            -_smoothLookInput.y,
            _smoothLookInput.x,
            _smoothRollInput);

        Vector3 rotationSpeed = new Vector3(
            _movementSettings.PitchSpeed,
            _movementSettings.YawSpeed,
            _movementSettings.RollSpeed);

        Vector3 rotationAmount = Vector3.Scale(rotationInput, rotationSpeed) * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(rotationAmount);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    private void RotateTowardsMovement()
    {
        float rollAmount = _smoothRollInput * _movementSettings.RollSpeed * Time.fixedDeltaTime;
        Quaternion currentRotation = _rigidbody.rotation;
        Quaternion rollRotation;

        if (_movementDirection.sqrMagnitude < 0.001f)
        {
            if (Mathf.Abs(_smoothRollInput) < 0.001f)
                return;
            Vector3 rollAxis = currentRotation * Vector3.forward;

            rollRotation =
                Quaternion.AngleAxis(
                    rollAmount,
                    rollAxis);
            _rigidbody.MoveRotation(rollRotation * _rigidbody.rotation);
            return;
        }

        Vector3 targetForward = _movementDirection.normalized;
        Vector3 currentUp = currentRotation * Vector3.up;

        Vector3 targetUp = Vector3.ProjectOnPlane(
            currentUp,
            targetForward);

        if (targetUp.sqrMagnitude < 0.001f)
        {
            targetUp = Vector3.ProjectOnPlane(
                    Vector3.up,
                    targetForward);
        }

        targetUp.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(
            targetForward,
            targetUp);

        Quaternion movementRotation = Quaternion.RotateTowards(
            currentRotation,
            targetRotation,
            _movementSettings.ThirdPersonTurnSpeed * Time.fixedDeltaTime);

        rollRotation = Quaternion.AngleAxis(rollAmount, targetForward);
        Quaternion finalRotation =
                   rollRotation *
                   movementRotation;
        _rigidbody.MoveRotation(finalRotation);
    }

    public void RotateTowardsShoot()
    {
        if (_movementDirection.sqrMagnitude < 0.001f)
            _rigidbody.MoveRotation(Quaternion.LookRotation(
                _cameraTransform.forward,
                Vector3.up));
    }
}

[Serializable]
public class ShipMovementSettings
{
    [Header("Movement")]

    [Tooltip("Tốc độ di chuyển tối đa của phi thuyền khi không sprint.")]
    [Range(0f, 100f)]
    public float MaxSpeed = 20f;

    [Tooltip("Tốc độ tăng tốc của phi thuyền.")]
    [Range(0f, 500f)]
    public float Acceleration = 35f;

    [Tooltip("Tốc độ giảm tốc khi thả phím di chuyển.")]
    [Range(0f, 500f)]
    public float Deceleration = 20f;

    [Tooltip("Hệ số nhân tốc độ khi giữ nút Sprint.")]
    [Range(1f, 5f)]
    public float SprintMultiplier = 2f;


    [Header("Rotation")]

    [Tooltip("Tốc độ xoay lên và xuống theo input Look.")]
    [Range(0f, 180f)]
    public float PitchSpeed = 45f;

    [Tooltip("Tốc độ xoay trái và phải theo input Look.")]
    [Range(0f, 180f)]
    public float YawSpeed = 45f;

    [Tooltip("Tốc độ lộn trái và phải theo input Roll.")]
    [Range(0f, 180f)]
    public float RollSpeed = 60f;


    [Header("Input Smoothing")]

    [Tooltip("Thời gian làm mượt input nhìn.")]
    [Range(0.01f, 0.5f)]
    public float LookSmoothing = 0.08f;

    [Tooltip("Thời gian làm mượt input di chuyển.")]
    [Range(0.01f, 0.5f)]
    public float MoveSmoothing = 0.08f;

    [Tooltip("Thời gian làm mượt input roll.")]
    [Range(0.01f, 0.5f)]
    public float RollSmoothing = 0.15f;


    [Header("Third Person")]

    [Tooltip("Tốc độ phi thuyền tự xoay theo hướng di chuyển trong góc nhìn thứ ba.")]
    [Range(0f, 360f)]
    public float ThirdPersonTurnSpeed = 180f;
}


