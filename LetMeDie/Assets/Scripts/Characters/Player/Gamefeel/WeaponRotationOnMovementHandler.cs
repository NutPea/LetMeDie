using System;
using UnityEngine;

public class WeaponRotationOnMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform weaponModels;
    private PlayerInput playerInput;
    private Vector2 movementInput;

    [Header("Horizontal")]
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private AnimationCurve horizontalSpeedCurve;

    [SerializeField] private float maxHorizontalRotation;

    private float horizontalMovementValue;

    [Header("Vertical")]
    [SerializeField] private float verticalMovementSpeed;
    [SerializeField] private AnimationCurve verticalSpeedCurve;

    [SerializeField] private float maxVerticalRotation;

    private float verticalMovementValue;


    void Start()
    {
        playerInput = SInputManager.Instance.inputActions;
        playerInput.Keyboard.Vertical.performed += VerticalMovement;
        playerInput.Keyboard.Vertical.canceled += VerticalMovement;
        playerInput.Keyboard.Horizontal.performed += HorizontalMovement;
        playerInput.Keyboard.Horizontal.canceled += HorizontalMovement;
    }

    private void HorizontalMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movementInput = new Vector2(playerInput.Keyboard.Horizontal.ReadValue<float>(), playerInput.Keyboard.Vertical.ReadValue<float>());
    }


    private void VerticalMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movementInput = new Vector2(playerInput.Keyboard.Horizontal.ReadValue<float>(), playerInput.Keyboard.Vertical.ReadValue<float>());
    }


    void Update()
    {
        horizontalMovementValue = Mathf.Clamp(horizontalMovementValue + movementInput.x * horizontalMovementSpeed * Time.deltaTime,-1,1);
        if (movementInput.x == 0)
        {
            horizontalMovementValue = Mathf.Lerp(horizontalMovementValue, 0, horizontalMovementSpeed * Time.deltaTime);
        }
        float horizontalRotation = 0;
           
        if(horizontalMovementValue > 0){
            horizontalRotation = Mathf.Lerp(0f, maxHorizontalRotation, horizontalSpeedCurve.Evaluate(horizontalMovementValue));
        }
        else{
            horizontalRotation = Mathf.Lerp(0f, -maxHorizontalRotation, horizontalSpeedCurve.Evaluate(Mathf.Abs(horizontalMovementValue)));
        }

        verticalMovementValue = Mathf.Clamp(verticalMovementValue + movementInput.y * verticalMovementSpeed * Time.deltaTime, -1, 1);
        if (movementInput.y == 0){
            verticalMovementValue = Mathf.Lerp(verticalMovementValue, 0, verticalMovementSpeed * Time.deltaTime);
        }
        float verticalRotation = 0;


        if (verticalMovementValue > 0) {
            verticalRotation = Mathf.Lerp(0f, maxVerticalRotation, verticalSpeedCurve.Evaluate(verticalMovementValue));
        }
        else{
            verticalRotation = Mathf.Lerp(0f, -maxVerticalRotation, verticalSpeedCurve.Evaluate(Mathf.Abs(verticalMovementValue)));
        }


        weaponModels.localEulerAngles = new Vector3(verticalRotation, 0, horizontalRotation);
    }
}
