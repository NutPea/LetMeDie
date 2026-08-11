using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private float fallingSpeed = 5f;
    [SerializeField] private Animator movementAnim;

    [SerializeField] private Animator swordAnimator;
    [SerializeField] private AnimationCurve swordAttackAnimationCurve;
    [SerializeField] private Animator bowAnimator;
    [SerializeField] private AnimationCurve bowAttackAnimationCurve;

    private Animator currentWeaponAnimator;
    private AnimationCurve currentWeaponAnimationCurve;

    private PlayerInput inputActions;
    private Vector2 playerInputVector;

    private bool hasFallen;
    private bool isSprinting;

    [SerializeField] private float xMoveAmount = 1;
    [SerializeField] private float zMoveAmount = 1;

    private PlayerMovementController playerMovementController;
    private PlayerInteractionHandler playerInteraction;
    private PlayerCombatController playerCombatController;
    private PlayerWeaponController playerWeaponController;
    private PlayerWeaponEquiper playerWeaponEquiper;

    private const string ATTACK_1 = "Attack_1";
    private const string ATTACK_2 = "Attack_2";

    private const string BLOCK_ATTACK = "BlockAttack";
    private const string EQUIP = "Equip";
    private const string UNEQUIP = "Unequip";

    private const string ISGROUNDED = "IsGrounded";
    private const string ISJUMPING = "IsJumping";

    private const float TIME_BETWEEN_ATTACK_INPUTS = 1f;
    private bool isAttack_1 = true;
    private bool hasSingleAttack;



    private void Start()
    {
        inputActions = SInputManager.Instance.inputActions;
        inputActions.Keyboard.Horizontal.performed += ctx =>  OnMove();
        inputActions.Keyboard.Vertical.performed += ctx => OnMove();
        inputActions.Keyboard.Horizontal.canceled += ctx => OnMove();
        inputActions.Keyboard.Vertical.canceled += ctx => OnMove();

        inputActions.Keyboard.Shift.performed += ctx => OnSprintStart();
        inputActions.Keyboard.Shift.canceled += ctx => OnSprintStop();

        inputActions.Keyboard.Jump.performed += ctx => OnJump();
    }


    private void Awake()
    {
        
        playerMovementController = GetComponent<PlayerMovementController>();

        playerMovementController.OnIsFalling.AddListener(OnFalling);
        playerMovementController.OnIsGrounded.AddListener(OnIsGrounded);
        playerMovementController.OnStartJump.AddListener(OnJump);

        playerInteraction = GetComponent<PlayerInteractionHandler>();
        playerInteraction.OnCanBeInteracted.AddListener(() => HandleInteract(true));
        playerInteraction.OnCanNotBeInteractedAnymore.AddListener(() => HandleInteract(false));
        playerInteraction.OnInteract.AddListener(OnInteract);

        playerCombatController = GetComponent<PlayerCombatController>();
        playerCombatController.OnEndCharge.AddListener(Attack);
        playerCombatController.OnCharge.AddListener(Charge);
        playerCombatController.OnStartBlock.AddListener(StartBlock);
        playerCombatController.OnEndBlock.AddListener(EndBlock);

        playerWeaponEquiper = GetComponent<PlayerWeaponEquiper>();
        playerWeaponEquiper.OnEquipWeapon.AddListener(EquipWeapon);
        isAttack_1 = true;

        playerWeaponController = GetComponent<PlayerWeaponController>();

    }

    public void UnEquip()
    {
        movementAnim.SetTrigger(UNEQUIP);
    }
    public void TriggerEquip()
    {
        movementAnim.SetTrigger(EQUIP);
    }

    private void EndBlock()
    {
        currentWeaponAnimator.SetBool("Block", false);
    }

    private void StartBlock()
    {
        currentWeaponAnimator.SetBool("Block", true);
    }

    private void EquipWeapon(WeaponData weaponData)
    {
        if (weaponData.GetType() == typeof(SwordData) || weaponData.GetType() == typeof(MagicWandData)) {
            currentWeaponAnimator = swordAnimator;
            currentWeaponAnimationCurve = swordAttackAnimationCurve;
            hasSingleAttack = false;
        }
        else if(weaponData.GetType() == typeof(BowData))
        {
            currentWeaponAnimator = bowAnimator;
            currentWeaponAnimationCurve = bowAttackAnimationCurve;
            hasSingleAttack = true;
        }
    }

    private void Attack(float chargeAmount)
    {
        if (!playerWeaponController.CanAttack)
        {
            return;
        }

        if (playerWeaponController.PlayerCombatController.IsBlocking)
        {
            currentWeaponAnimator.SetTrigger(BLOCK_ATTACK);
            return;
        }

        CancelInvoke(nameof(ResetToAttack_1));
        if (isAttack_1 || hasSingleAttack)
        {
            currentWeaponAnimator.SetBool(ATTACK_1, true);
            isAttack_1 = false;
        }
        else
        {
            float randomAttack = UnityEngine.Random.Range(0.0f, 1.0f);
            if(randomAttack < 0.5f)
            {
                currentWeaponAnimator.SetBool(ATTACK_1, true);
            }
            else
            {
                currentWeaponAnimator.SetBool(ATTACK_2, true);
            }
        }

        currentWeaponAnimator.SetFloat("Charge", 0);


        Invoke(nameof(ResetAttack), 0.1f);
        Invoke(nameof(ResetToAttack_1), TIME_BETWEEN_ATTACK_INPUTS);
    }

    private void ResetToAttack_1()
    {
        isAttack_1 = true;
    }

    private void ResetAttack()
    {
        currentWeaponAnimator.SetBool(ATTACK_1, false);
        currentWeaponAnimator.SetBool(ATTACK_2, false);
    }

    private void Charge(float chargeAmount)
    {
        currentWeaponAnimator.SetFloat("Charge", currentWeaponAnimationCurve.Evaluate(chargeAmount));
    }

    private void HandleInteract(bool interact)
    {
       // anim.SetBool("CanInteract", interact);
    }

    private void OnInteract()
    {
       // anim.SetTrigger("Interact");
    }
  

    private void OnIsGrounded(bool isGrounded)
    {
        movementAnim.SetBool(ISGROUNDED, isGrounded);
        if (!isGrounded)
        {
            movementAnim.SetBool(ISJUMPING, false);
        }
    }

    private void OnJump()
    {
        movementAnim.SetBool(ISJUMPING,true);
    }

    private void OnSprintStart()
    {
        isSprinting = true;
    }

    private void OnSprintStop()
    {
        isSprinting = false;
    }

    
    private void OnFalling(float fallingSpeedValue)
    {
        if(fallingSpeedValue < fallingSpeed)
        {
            if (!hasFallen)
            {
               // anim.SetTrigger("IsFalling");
                hasFallen = true;
            }

        }
        else
        {
            if (hasFallen)
            {
                hasFallen = false;
            }
        }
    }

    private void OnMove()
    {
        playerInputVector = new Vector2(inputActions.Keyboard.Horizontal.ReadValue<float>(), inputActions.Keyboard.Vertical.ReadValue<float>());
        playerInputVector = playerInputVector.normalized;
        movementAnim.SetFloat("Movement", playerInputVector.magnitude);
        // anim.SetBool("isWalking", playerInputVector != Vector2.zero);

        // weaponTransform.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //weaponTransform.transform.localRotation = Quaternion.Euler(new Vector3(playerInputVector.y* zMoveAmount, 180, playerInputVector.x * xMoveAmount));
    }


    private void Update()
    {
        if (isSprinting)
        {
            if (playerInputVector == Vector2.zero)
            {
               // anim.SetBool("isSprinting", false);
            }
            else
            {
               // anim.SetBool("isSprinting", true);
            }
        }
        else
        {
          //  anim.SetBool("isSprinting", false);
        }
    }


}
