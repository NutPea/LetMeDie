using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombatController : MonoBehaviour
{

    private PlayerInput inputActions;

    public UnityEvent OnStartCharge = new UnityEvent();
    [HideInInspector]public UnityEvent<float> OnCharge = new();
    [HideInInspector]public UnityEvent<float> OnEndCharge = new();

    [HideInInspector] public UnityEvent OnStartBlock = new();
    [HideInInspector] public UnityEvent OnEndBlock = new();

    private bool isCharging;
    [SerializeField] private float currentFullChargeTime = 1f;
    public float CurrentFullChargeTime
    {
        set { currentFullChargeTime = value; }
    }
    private float currentChargeAmount;
    private float CurrentChargePercentage => currentChargeAmount / currentFullChargeTime; 
    private HealthManager healthManager;
    private PlayerWeaponController playerWeaponController;

    [Header("FOV Change")]
    [HideInInspector] public bool CanChangeFOVOnCharge = false;
    private float startFOV = 90f;
    [SerializeField] private float minFOV = 60f;
    [SerializeField] private AnimationCurve fovAnimationCurve;

    [Header("CameraShake")]
    [SerializeField] private Vector2 intensity;
    [SerializeField] private Vector2 frequence;
    [SerializeField] private Vector2 shakeTime;

    private bool isBlocking;
    public bool IsBlocking => isBlocking;
    private bool CanUseCombat => !SGameManager.IsPaused;

    private bool attackCooldownTrigger;

    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
    }

    void Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.Attack.performed += StartCharging;
        inputActions.Keyboard.Attack.canceled += EndCharging;

        inputActions.Keyboard.Block.performed += StartBlock;
        inputActions.Keyboard.Block.canceled += EndBlock;

        startFOV = SCameraShake.Instance.CurrentlyUsedCamera.Lens.FieldOfView;
    }

    private void EndBlock(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!CanUseCombat)
        {
            return;
        }

        if (!playerWeaponController.CanBlock || isCharging)
        {
            return;
        }
        OnEndBlock.Invoke();
        healthManager.isBlocked = false;
        isBlocking = false;
    }

    private void StartBlock(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!CanUseCombat)
        {
            return;
        }

        if (!playerWeaponController.CanBlock || isCharging)
        {
            return;
        }
        OnStartBlock.Invoke();
        healthManager.isBlocked = true;
        isCharging = false;
        currentChargeAmount = 0f;
        isBlocking = true;
    }

    private void EndCharging(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!playerWeaponController.CanAttack)
        {
            attackCooldownTrigger = false;
            return;
        }
        if (!CanUseCombat)
        {
            return;
        }
        if (isBlocking)
        {
            return;
        }
        OnEndCharge.Invoke(CurrentChargePercentage);
        isCharging = false;
        SCameraShake.Instance.ShakeForSeconds(Mathf.Lerp(intensity.x, intensity.y, CurrentChargePercentage),
            Mathf.Lerp(frequence.x, frequence.y, CurrentChargePercentage), 
            Mathf.Lerp(shakeTime.x, shakeTime.y, CurrentChargePercentage));

        if (CanChangeFOVOnCharge){
            SCameraShake.Instance.ChangeFOV(startFOV);
        }

    }

    private void StartCharging(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {


        if (!playerWeaponController.CanAttack)
        {
            attackCooldownTrigger = true;
            return;
        }

        if (!CanUseCombat)
        {
            return;
        }
        if (isBlocking)
        {
            OnEndCharge.Invoke(0);
            return;
        }
        SetCharging();
    }

    private void SetCharging()
    {
        if (CanChangeFOVOnCharge){
            SCameraShake.Instance.ChangeFOV(startFOV);
        }
        isCharging = true;
        currentChargeAmount = 0f;
        OnStartCharge.Invoke();
        attackCooldownTrigger = false;
    }

    private void Update()
    {
        if (attackCooldownTrigger && playerWeaponController.CanAttack)
        {
            SetCharging();
            attackCooldownTrigger = false;
        }

        if (!isCharging || !CanUseCombat)
        {
            return;
        }

        if(currentChargeAmount > currentFullChargeTime)
        {
            return;
        }

        currentChargeAmount += Time.deltaTime;
        OnCharge.Invoke(CurrentChargePercentage);
        if (CanChangeFOVOnCharge){
            SCameraShake.Instance.ChangeFOV(Mathf.Lerp(startFOV, minFOV, fovAnimationCurve.Evaluate(currentChargeAmount)));
        }
    }

}
