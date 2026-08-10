using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInput inputActions;
    private Transform mainCameraTransform;
    private Vector2 playerInputVector;
    private PlayerStatHandler playerStatHandler;

    private bool CanNotUse => SGameManager.IsInDialog || SGameManager.IsPaused;

    public float currentMovementSpeed;
    private Rigidbody rb;
    public enum MovementState { Walking , Sprinting, Air, Crouching,Sliding}
    public MovementState currentMovementState = MovementState.Walking;

    [Header("WalkingAndSprinting")]
    public float baseMovementSpeed = 50;
    private float calculatedMovementSpeed;
    private bool isSprinting;
    public float groundDrag = 2;

    [Header("Jumping")]
    public float baseJumpForce = 17;
    private float calculatedJumpForce;
    public float jumpCooldown = 1f;
    public float airMultiplierWhenCancleJump = 0.5f;
    bool canJump;
    public float baseAirMovementSpeed = 2f;
    private float calculatedAirMovementSpeed = 0f;
    public float extraGravity = 0.0f;
    public float currentExtraGravity = 0.0f;

    public float jumpBufferTime = 0.1f;
    private bool jumpIsBuffered;

    [Header("Crouching")]
    public Transform capsuleCollider;
    public Transform ovverideCamPosition;
    public float mainCamStartPos;
    public float timeToCrouch = 1f;
    private float currentTimeToCrouch;
    public float crouchMovementSpeed;
    public float crouchYScale;
    public float crouchDrag;
    bool isCrouching;
    bool startCrouching;
    bool startStandUp;

    [Header("Slope Handling")]
    public float minSlopeAngle = 20f;
    public float maxSlopeAngle = 40;
    public float slopePercentage = 0.0f;
    private RaycastHit slopeHit;

    [Header("Sliding")]
    public float addingSlidingSpeedValuePerSecond = 10f;
    public float currentSlidingTime;
    public float maxSlidingTime = 3f;
    public float momentumTime = 2f;

    [Header("Ground Check")]
    public float playerHeight;
    public float playerCheckRadius;
    public LayerMask groundLayerMask;
    [HideInInspector]public bool isGrounded;
    public float coyoteTime = 0.5f;
    private bool isCoyoteTimeGrounded;
    private bool wasGrounded;
    private bool hasJumped;
    private bool wasFalling;
    public float wasFallingVelocity = 10;

    bool keepMomentum;
    bool hasSlided;

    //Events
    [HideInInspector]
    public UnityEvent OnIsCrouching;
    public UnityEvent OnIsSliding;
    public UnityEvent OnIsWalking;
    public UnityEvent OnIsSprinting;

    public UnityEvent<float> OnIsFalling = new UnityEvent<float>();
    public UnityEvent<bool> OnIsGrounded = new UnityEvent<bool>();

    public UnityEvent OnStartJump;
    public UnityEvent OnLandAfterJump;

    private void Awake()
    {
        calculatedMovementSpeed = baseMovementSpeed;
        calculatedJumpForce = baseJumpForce;
        calculatedAirMovementSpeed = baseAirMovementSpeed;

        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerStatHandler.OnStatUpdate.AddListener(RecalculateMovementStats);
    }

    private void RecalculateMovementStats(PlayerData playerStats)
    {
        calculatedMovementSpeed = PlayerData.CalculateMovementSpeed(baseMovementSpeed, playerStats.Speed);
        calculatedJumpForce = PlayerData.CalculateJumpForce(baseJumpForce, playerStats.Dexterity);
        calculatedAirMovementSpeed = PlayerData.CalculateAirMovementSpeed(baseAirMovementSpeed, playerStats.Dexterity);
    }

    private void Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.Jump.performed += ctx => PerformJump();
        inputActions.Keyboard.Jump.canceled += ctx => CancleJump();

        inputActions.Keyboard.Crouch.performed += ctx => StartCrouch();
        inputActions.Keyboard.Crouch.canceled += ctx => StopCrouch();
        mainCameraTransform = Camera.main.transform;
        mainCamStartPos = mainCameraTransform.localPosition.y;
        rb = GetComponent<Rigidbody>();

        canJump = true;
    }
    private void StartCrouch()
    {
        if (CanNotUse)
        {
            return;
        }
        isCrouching = true;
        startCrouching = true;
        currentTimeToCrouch = timeToCrouch;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void StopCrouch()
    {
        if (CanNotUse)
        {
            return;
        }
        currentTimeToCrouch = timeToCrouch;
        isCrouching = false;
        startCrouching = false;
        startStandUp = true;
    }

    public void SetCrouchScale(float scale)
    {
        capsuleCollider.transform.localScale = new Vector3(capsuleCollider.transform.localScale.x, scale, capsuleCollider.transform.localScale.z);
    }

    private void Update()
    {
        if (CanNotUse)
        {
            return;
        }

        if (startCrouching)
        {
            if(currentTimeToCrouch <= 0)
            {
                currentTimeToCrouch = timeToCrouch;
                SetCrouchScale(crouchYScale);
                startCrouching = false;
            }
            else
            {
                currentTimeToCrouch -= Time.deltaTime;
                float lerpValue = Mathf.Lerp(crouchYScale, 1f, currentTimeToCrouch / timeToCrouch);
                SetCrouchScale(lerpValue);
            }
        }

        if (startStandUp)
        {
            if (currentTimeToCrouch <= 0)
            {
                currentTimeToCrouch = timeToCrouch;
                SetCrouchScale(1f);
                mainCameraTransform.transform.localPosition = new Vector3(mainCameraTransform.transform.localPosition.x, mainCamStartPos, mainCameraTransform.transform.localPosition.z);
                startStandUp = false;
            }
            else
            {
                currentTimeToCrouch -= Time.deltaTime;
                float lerpValue = Mathf.Lerp(1.0f, crouchYScale, currentTimeToCrouch / timeToCrouch);
                SetCrouchScale(lerpValue);
                mainCameraTransform.transform.position = new Vector3(mainCameraTransform.transform.position.x, ovverideCamPosition.position.y, mainCameraTransform.transform.position.z);
            }
        }
        if (isCrouching)
        {
            mainCameraTransform.transform.position = new Vector3(mainCameraTransform.transform.position.x, ovverideCamPosition.position.y, mainCameraTransform.transform.position.z);
        }



        if(isCrouching && OnSlope() && rb.linearVelocity.y < 0.1f)
        {
            currentMovementState = MovementState.Sliding;
            hasSlided= true;
        }
        else if (isCrouching){
            currentMovementState = MovementState.Crouching;
        }
        else if (isSprinting){
            currentMovementState = MovementState.Sprinting;
        }
        else if (isGrounded){
            currentMovementState = MovementState.Walking;
        }
        else{
            currentMovementState = MovementState.Air;
        }

        if (!OnSlope())
        {
            if (currentMovementState != MovementState.Sliding || rb.linearVelocity.y > 0.1f || playerInputVector == Vector2.zero && !keepMomentum)
            {
                if (hasSlided)
                {
                    currentSlidingTime = 0;
                    keepMomentum = true;
                    StopCoroutine(SmoothLerpMovementSpeed(momentumTime));
                    StartCoroutine(SmoothLerpMovementSpeed(momentumTime));
                    hasSlided = false;
                }
            }
        }

        if(currentMovementState != MovementState.Sliding)
        {
            currentSlidingTime = 0;
        }
         
        isGrounded = OnGround();
        OnIsGrounded.Invoke(isGrounded);
        if (isGrounded){

            if (jumpIsBuffered && canJump)
            {
                Jump();
                ResetJumpBuffer();
            }

            if(isCrouching)
            {
                rb.linearDamping = crouchDrag;
            }
            else
            {
                rb.linearDamping = groundDrag;
            }

            currentExtraGravity = 0.0f;
            if (!wasGrounded && (hasJumped || wasFalling) && currentMovementState != MovementState.Sliding )
            {
                OnLandAfterJump.Invoke();
                wasGrounded = true;
                wasFalling = false;
                hasJumped = false;
            }
            isCoyoteTimeGrounded = true;
        }
        else{
            if (wasGrounded) {
                Invoke(nameof(ResetCoyoteTimeGrounded), coyoteTime);
                wasGrounded = false;
            }
            if(rb.linearVelocity.y < 0)
            {
                OnIsFalling.Invoke(rb.linearVelocity.y);
            }
            rb.linearDamping = 0;
        }

    }


    void FixedUpdate()
    {
        if (CanNotUse)
        {
            return;
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        rb.useGravity = true;
        playerInputVector = new Vector2(inputActions.Keyboard.Horizontal.ReadValue<float>(), inputActions.Keyboard.Vertical.ReadValue<float>());
        playerInputVector = playerInputVector.normalized;

        if (!keepMomentum)
        {
            switch (currentMovementState)
            {
                case MovementState.Walking: currentMovementSpeed = calculatedMovementSpeed; break;
                case MovementState.Crouching: currentMovementSpeed = crouchMovementSpeed; break;
                case MovementState.Sliding:
                    {
                        currentMovementSpeed = currentSlidingTime * addingSlidingSpeedValuePerSecond * slopePercentage;
                        currentSlidingTime += Time.deltaTime;
                        if (currentSlidingTime > maxSlidingTime)
                        {
                            currentSlidingTime = maxSlidingTime;
                        }
                        break;
                    }
            }
        }



        if (isGrounded)
        {
            if (OnSlope()){
                if(currentMovementState == MovementState.Sliding){
                    OnIsSliding.Invoke();
                    Vector3 horizontalMovementVec = playerInputVector.x * mainCameraTransform.right;
                    horizontalMovementVec = horizontalMovementVec.normalized;
                    rb.AddForce((-GetSlopeMoveDirection() +horizontalMovementVec) * currentMovementSpeed , ForceMode.Force);
                }
            }
            else{
                Vector3 horPlayerMovement = mainCameraTransform.right * playerInputVector.x * currentMovementSpeed;
                Vector3 vertPlayerMovement = transform.forward * -playerInputVector.y * currentMovementSpeed;
                if (playerInputVector == Vector2.zero)
                {
                    if(rb.linearVelocity.magnitude < 1)
                    {
                        rb.useGravity = false;
                    }
                }
                rb.AddForce(horPlayerMovement + vertPlayerMovement, ForceMode.Force);
            }
        }
        else
        {
            Vector3 horPlayerMovement = mainCameraTransform.right * playerInputVector.x * baseAirMovementSpeed;
            Vector3 vertPlayerMovement = transform.forward * -playerInputVector.y * baseAirMovementSpeed;
            rb.AddForce(horPlayerMovement + vertPlayerMovement, ForceMode.Force);

            currentExtraGravity += extraGravity * Time.fixedDeltaTime;
            rb.AddForce(currentExtraGravity * Vector3.down);
        }


        if(playerInputVector != Vector2.zero && isGrounded)
        {
            switch (currentMovementState)
            {
                case MovementState.Sprinting: OnIsSprinting.Invoke(); break;
                case MovementState.Walking: OnIsWalking.Invoke(); break;
                case MovementState.Crouching: OnIsCrouching.Invoke(); break;
            }
        }

        if(rb.linearVelocity.y < wasFallingVelocity && !wasFalling)
        {
            wasFalling = true;
        }


    }

    private void SpeedControll()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(flatVel.magnitude > baseMovementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * baseMovementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, limitedVel.y,limitedVel.z);
        }
    }

    private IEnumerator SmoothLerpMovementSpeed(float momentumTime)
    {
        float time = 0;
        float startValue = currentMovementSpeed;
        while(time < momentumTime)
        {
            currentMovementSpeed = Mathf.Lerp(startValue, baseMovementSpeed,time/ momentumTime);
            time += Time.deltaTime * momentumTime;
            yield return null;
        }
        keepMomentum= false;
    }

    private void PerformJump()
    {
        if (CanNotUse)
        {
            return;
        }
        if(!canJump) { 
            return;
        }
        if(isCoyoteTimeGrounded)
        {
            Jump();
        }
        else if (!isGrounded)
        {
            Invoke(nameof(ResetJumpBuffer), jumpBufferTime);
            jumpIsBuffered = true;
        }
    }

    private void CancleJump()
    {
        if (CanNotUse)
        {
            return;
        }
        if (canJump) { 
            return;
        }
        if(rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,rb.linearVelocity.y * airMultiplierWhenCancleJump,rb.linearVelocity.z);
        }

    }

    private void Jump()
    {
        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
        ResetCoyoteTimeGrounded();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * calculatedJumpForce, ForceMode.Impulse);

        OnStartJump.Invoke();
        hasJumped = true;
    }

    private void ResetJumpBuffer()
    {
        jumpIsBuffered = false;
    }

    private void ResetCoyoteTimeGrounded()
    {
        isCoyoteTimeGrounded = false;
    }

    private void ResetJump()
    {
        canJump = true;
    }
    private void StartSprinting()
    {
        isSprinting = true;
    }

    private void StopSprinting()
    {
        isSprinting = false;
    }

    private bool OnGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask))
        {
            return true;
        }
        if (Physics.Raycast(transform.position+ new Vector3(playerCheckRadius, 0, 0), Vector3.down, (playerHeight - 0.1f), groundLayerMask))
        {
            return true;
        }
        if (Physics.Raycast(transform.position + new Vector3(-playerCheckRadius, 0, 0), Vector3.down, (playerHeight - 0.1f), groundLayerMask))
        {
            return true;
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 0, playerCheckRadius), Vector3.down, (playerHeight - 0.1f), groundLayerMask))
        {
            return true;
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 0, -playerCheckRadius), Vector3.down, (playerHeight - 0.1f), groundLayerMask))
        {
            return true;
        }

        return false;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit, playerHeight * 0.5f + 0.3f, groundLayerMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            slopePercentage = (angle - minSlopeAngle) / (maxSlopeAngle - minSlopeAngle);
            if(slopePercentage < 0f){
                slopePercentage = 0f;
            }
            return angle > minSlopeAngle && angle <= maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        Vector3 slopeDir = Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);
        return slopeDir;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        Vector3 horizontalMovementVec = playerInputVector.x * mainCameraTransform.right;
        Vector3 verticalMovementVec = -playerInputVector.y * transform.forward;

        return Vector3.ProjectOnPlane(horizontalMovementVec + verticalMovementVec, slopeHit.normal).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * playerHeight);
        Gizmos.DrawLine(transform.position, transform.position+ new Vector3(playerCheckRadius, 0, 0)+ Vector3.down * (playerHeight-0.2f));
        Gizmos.DrawLine(transform.position, transform.position+new Vector3(-playerCheckRadius, 0, 0) + Vector3.down * (playerHeight - 0.2f));

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, playerCheckRadius) + Vector3.down * (playerHeight - 0.2f));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, -playerCheckRadius) + Vector3.down * (playerHeight - 0.2f));
    }
}
