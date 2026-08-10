using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class PlayerDash : MonoBehaviour
{
    PlayerInput inputActions;
    bool canDash;
    public float dashForce;
    public float dashTimePercentage;
    private Transform cameraTransform;
    private Rigidbody rb;
    private PlayerMovementController playerMovementController;

    public float slowDownTime = 1f;
    private float currentSlowDownTime = 1f;
    [HideInInspector] public bool resetTime;

    [HideInInspector] public UnityEvent onTimeHasStopped = new UnityEvent();
    [HideInInspector] public UnityEvent onTimeHasReseted = new UnityEvent();




    void Start()
    {
        inputActions= SInputManager.Instance.inputActions;
        playerMovementController = GetComponent<PlayerMovementController>();
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        inputActions.Keyboard.Jump.performed += ctx => OnDash();
        currentSlowDownTime = slowDownTime;
    }

    private void OnDash()
    {
        if (!canDash){
            return;
        }

        if (playerMovementController.isGrounded)
        {
            canDash = false;
            return;
        }

        ResetTime();
        playerMovementController.currentExtraGravity = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(cameraTransform.forward * dashForce,ForceMode.Impulse);
        canDash = false;
    }

    private void Update()
    {
        if (resetTime)
        {
            if(currentSlowDownTime < 0)
            {
                ResetTime();
                dashTimePercentage= 0;
                resetTime= false;
            }
            else
            {
                currentSlowDownTime -= Time.unscaledDeltaTime;
                dashTimePercentage = currentSlowDownTime / slowDownTime;
            }
        }
    }


    void ResetTime()
    {
        Time.timeScale = 1;
        currentSlowDownTime = slowDownTime;
        canDash = false;
        resetTime = false;
        onTimeHasReseted.Invoke();
    }


    public void EnableDash()
    {
        canDash = true;
        Time.timeScale = 0;
        resetTime= true;
        onTimeHasStopped.Invoke();
    }

}
