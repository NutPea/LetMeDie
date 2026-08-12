using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterControllerMovementController : MonoBehaviour
{
    private PlayerInput inputActions;
    private Transform mainCameraTransform;
    private CharacterController characterController;

    private Vector2 playerInputVector;

    // =========================================================
    // EVENTS
    // =========================================================

    [Header("Movement Events")]
    public UnityEvent<float> OnIsFalling = new UnityEvent<float>();
    public UnityEvent<bool> OnIsGrounded = new UnityEvent<bool>();

    public UnityEvent OnStartJump = new UnityEvent();
    public UnityEvent OnLandAfterJump = new UnityEvent();

    private bool wasGrounded;
    private bool wasJumping;

    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 6f;

    [Tooltip("Movement control while airborne. 1 = full control.")]
    [SerializeField] private float airControl = 1f;

    // =========================================================
    // JUMP
    // =========================================================

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Tooltip("How long after leaving the ground the player can still jump.")]
    [SerializeField] private float coyoteTime = 0.15f;

    [Tooltip("How long a jump input is buffered before landing.")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Tooltip("Reduces jump height when the jump button is released early.")]
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    private float coyoteTimer;
    private float jumpBufferTimer;

    // =========================================================
    // GRAVITY
    // =========================================================

    [Header("Gravity")]
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float extraGravity = 2f;

    [Tooltip(
        "Controls how gravity increases over time.\n" +
        "X = normalized time\n" +
        "Y = gravity multiplier"
    )]
    [SerializeField]
    private AnimationCurve gravityCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 0.5f),
        new Keyframe(1f, 1f)
    );

    [Tooltip("Time it takes for the gravity curve to reach its end.")]
    [SerializeField] private float gravityRampDuration = 1f;

    [Tooltip("Maximum falling speed.")]
    [SerializeField] private float maxFallSpeed = 30f;

    private float gravityTime;
    private float verticalVelocity;

    // =========================================================
    // DASH
    // =========================================================

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private float dashTimer;
    private float dashCooldownTimer;

    private Vector3 dashDirection;

    // =========================================================
    // KNOCKBACK
    // =========================================================

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;

    private Vector3 knockbackVelocity;
    private float knockbackTimer;

    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        inputActions = SInputManager.Instance.inputActions;

        mainCameraTransform = Camera.main.transform;

        inputActions.Keyboard.Jump.performed += PerformJump;
        inputActions.Keyboard.Jump.canceled += CancelJump;

        inputActions.Keyboard.Horizontal.performed += MovementInput;
        inputActions.Keyboard.Horizontal.canceled += MovementInput;

        inputActions.Keyboard.Vertical.performed += MovementInput;
        inputActions.Keyboard.Vertical.canceled += MovementInput;

        inputActions.Keyboard.Dash.performed += Dash;

        wasGrounded = characterController.isGrounded;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        TryDash();
    }

    private void OnDestroy()
    {
        if (inputActions == null)
            return;

        inputActions.Keyboard.Jump.performed -= PerformJump;
        inputActions.Keyboard.Jump.canceled -= CancelJump;

        inputActions.Keyboard.Horizontal.performed -= MovementInput;
        inputActions.Keyboard.Horizontal.canceled -= MovementInput;

        inputActions.Keyboard.Vertical.performed -= MovementInput;
        inputActions.Keyboard.Vertical.canceled -= MovementInput;
    }

    private void Update()
    {
        UpdateTimers();

        HandleJump();

        Vector3 movement = CalculateMovement();

        movement += CalculateDashMovement();
        movement += CalculateKnockbackMovement();
        movement += CalculateGravity();

        characterController.Move(movement);

        UpdateGroundedState();
        UpdateFallingState();
    }

    // =========================================================
    // INPUT
    // =========================================================

    private void MovementInput(InputAction.CallbackContext context)
    {
        playerInputVector = new Vector2(
            inputActions.Keyboard.Horizontal.ReadValue<float>(),
            -inputActions.Keyboard.Vertical.ReadValue<float>()
        );

        playerInputVector = Vector2.ClampMagnitude(
            playerInputVector,
            1f
        );
    }

    private void PerformJump(InputAction.CallbackContext context)
    {
        // Store the input.
        // The actual jump will happen in HandleJump().
        jumpBufferTimer = jumpBufferTime;
    }

    private void CancelJump(InputAction.CallbackContext context)
    {
        // Early jump release.
        if (verticalVelocity > 0f)
        {
            verticalVelocity *= jumpCutMultiplier;
        }
    }

    // =========================================================
    // TIMERS
    // =========================================================

    private void UpdateTimers()
    {
        float deltaTime = Time.deltaTime;

        // -----------------------------------------------------
        // Jump Buffer
        // -----------------------------------------------------

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= deltaTime;
        }

        // -----------------------------------------------------
        // Coyote Time
        // -----------------------------------------------------

        if (characterController.isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= deltaTime;
        }

        // -----------------------------------------------------
        // Dash Cooldown
        // -----------------------------------------------------

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= deltaTime;
        }

        // -----------------------------------------------------
        // Dash Timer
        // -----------------------------------------------------

        if (dashTimer > 0f)
        {
            dashTimer -= deltaTime;
        }

        // -----------------------------------------------------
        // Knockback Timer
        // -----------------------------------------------------

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= deltaTime;
        }
    }

    // =========================================================
    // MOVEMENT
    // =========================================================

    private Vector3 CalculateMovement()
    {
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        // Ignore camera pitch.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movementDirection =
            right * playerInputVector.x +
            forward * playerInputVector.y;

        movementDirection = Vector3.ClampMagnitude(
            movementDirection,
            1f
        );

        float currentSpeed = movementSpeed;

        if (!characterController.isGrounded)
        {
            currentSpeed *= airControl;
        }

        return movementDirection *
               currentSpeed *
               Time.deltaTime;
    }

    // =========================================================
    // JUMP
    // =========================================================

    private void HandleJump()
    {
        if (jumpBufferTimer <= 0f)
            return;

        if (coyoteTimer <= 0f)
            return;

        PerformActualJump();
    }

    private void PerformActualJump()
    {
        // Consume jump buffer.
        jumpBufferTimer = 0f;

        // Consume coyote time.
        coyoteTimer = 0f;

        // Apply jump velocity.
        verticalVelocity = jumpForce;

        // Restart gravity curve.
        gravityTime = 0f;

        // Remember that we are currently jumping.
        wasJumping = true;

        // Notify listeners.
        OnStartJump?.Invoke();
    }

    // =========================================================
    // GRAVITY
    // =========================================================

    private Vector3 CalculateGravity()
    {
        float deltaTime = Time.deltaTime;

        // Keep the player slightly grounded.
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
            gravityTime = 0f;

            return Vector3.up *
                   verticalVelocity *
                   deltaTime;
        }

        // Increase gravity time.
        gravityTime += deltaTime;

        float normalizedGravityTime =
            Mathf.Clamp01(
                gravityTime / gravityRampDuration
            );

        float gravityMultiplier =
            gravityCurve.Evaluate(
                normalizedGravityTime
            );

        float currentGravity =
            gravity + extraGravity * gravityMultiplier;

        verticalVelocity -=
            currentGravity * deltaTime;

        verticalVelocity = Mathf.Max(
            verticalVelocity,
            -maxFallSpeed
        );

        return Vector3.up *
               verticalVelocity *
               deltaTime;
    }

    // =========================================================
    // GROUND STATE
    // =========================================================

    private void UpdateGroundedState()
    {
        bool isGrounded = characterController.isGrounded;

        // Only invoke the event when the state changes.
        if (isGrounded != wasGrounded)
        {
            OnIsGrounded?.Invoke(isGrounded);

            // We landed after a jump.
            if (isGrounded && wasJumping)
            {
                OnLandAfterJump?.Invoke();

                wasJumping = false;
            }

            wasGrounded = isGrounded;
        }
    }

    // =========================================================
    // FALLING
    // =========================================================

    private void UpdateFallingState()
    {
        // Player is falling.
        if (!characterController.isGrounded &&
            verticalVelocity < 0f)
        {
            // Convert negative velocity into a positive
            // falling speed for easier Animator usage.
            float fallingSpeed =
                Mathf.Abs(verticalVelocity);

            OnIsFalling?.Invoke(fallingSpeed);
        }
    }

    // =========================================================
    // DASH
    // =========================================================

    /// <summary>
    /// Attempts to start a dash.
    /// Dash only works while grounded.
    /// </summary>
    public bool TryDash()
    {
        // Dash only works on the ground.
        if (!characterController.isGrounded)
            return false;

        // Still on cooldown.
        if (dashCooldownTimer > 0f)
            return false;

        // Already dashing.
        if (dashTimer > 0f)
            return false;

        dashDirection = GetDashDirection();

        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        return true;
    }

    private Vector3 GetDashDirection()
    {
        Vector3 direction;

        if (playerInputVector.sqrMagnitude > 0.01f)
        {
            Vector3 forward = mainCameraTransform.forward;
            Vector3 right = mainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            direction =
                right * playerInputVector.x +
                forward * playerInputVector.y;
        }
        else
        {
            direction = transform.forward;
        }

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction = transform.forward;
        }

        return direction.normalized;
    }

    private Vector3 CalculateDashMovement()
    {
        if (dashTimer <= 0f)
            return Vector3.zero;

        return dashDirection *
               dashSpeed *
               Time.deltaTime;
    }

    // =========================================================
    // KNOCKBACK
    // =========================================================

    /// <summary>
    /// Applies a knockback to the character.
    /// </summary>
    /// <param name="direction">
    /// Direction the character should be pushed.
    /// </param>
    /// <param name="force">
    /// Horizontal knockback force.
    /// </param>
    /// <param name="verticalForce">
    /// Vertical knockback force.
    /// </param>
    public void ApplyKnockback(
        Vector3 direction,
        float force,
        float verticalForce = 0f)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();
        }

        knockbackVelocity =
            direction * force;

        knockbackVelocity.y =
            verticalForce;

        knockbackTimer =
            knockbackDuration;
    }

    private Vector3 CalculateKnockbackMovement()
    {
        if (knockbackTimer <= 0f)
            return Vector3.zero;

        // Fade knockback over time.
        float normalizedTime =
            knockbackTimer /
            knockbackDuration;

        Vector3 currentVelocity =
            knockbackVelocity *
            normalizedTime;

        return currentVelocity *
               Time.deltaTime;
    }

    // =========================================================
    // PUBLIC API
    // =========================================================

    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    public bool IsDashing()
    {
        return dashTimer > 0f;
    }

    public bool IsBeingKnockedBack()
    {
        return knockbackTimer > 0f;
    }

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }

    public float GetFallingSpeed()
    {
        if (verticalVelocity >= 0f)
            return 0f;

        return Mathf.Abs(verticalVelocity);
    }
}