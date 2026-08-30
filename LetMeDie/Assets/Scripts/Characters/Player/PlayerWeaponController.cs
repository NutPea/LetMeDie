using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    private PlayerCombatController playerCombatController;
    public PlayerCombatController PlayerCombatController => playerCombatController;
    private WeaponData weaponData;
    [SerializeField]private Transform cameraTransform;
    private PlayerStatHandler playerStatHandler;
    public PlayerData PlayerData => playerStatHandler.PlayerData;

    private PlayerResourceHandler playerResourceHandler;

    private bool HasSomethingEquiped;
    public bool CanBlock => weaponData.CanBlock;

    private float currentAttackCooldown = 0f;
   public bool CanAttack;
    private float currentChargeAmount;

    void Awake()
    {
        playerCombatController = GetComponent<PlayerCombatController>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerResourceHandler = GetComponent<PlayerResourceHandler>();

    }

    private void Start()
    {
        SInputManager.Instance.inputActions.Keyboard.Spell_1.performed += SpellCast_1;
        SInputManager.Instance.inputActions.Keyboard.Spell_2.performed += SpellCast_2;
        SInputManager.Instance.inputActions.Keyboard.Spell_3.performed += SpellCast_3;
        playerCombatController.OnEndCharge.AddListener(Attack);
    }


    private void SpellCast_3(InputAction.CallbackContext context)
    {
        MagicSpell spell = playerStatHandler.PlayerData.CurrentMagicSpell_3;
        if (spell != null)
        {
            spell.Attack(cameraTransform, 1);
        }
    }

    private void SpellCast_2(InputAction.CallbackContext context)
    {
        MagicSpell spell = playerStatHandler.PlayerData.CurrentMagicSpell_2;
        if (spell != null) {
            spell.Attack(cameraTransform, 1);
        }
    }

    private void SpellCast_1(InputAction.CallbackContext context)
    {
        MagicSpell spell = playerStatHandler.PlayerData.CurrentMagicSpell_1;
        if (spell != null)
        {
            spell.Attack(cameraTransform, 1);
        }
    }

    private void Attack(float chargeAmount)
    {
        if (weaponData == null || !CanAttack) {
            return;
        }
        currentChargeAmount = chargeAmount;
        currentAttackCooldown = weaponData.AttackCooldown;
        CanAttack = false;
        if (!weaponData.AttackWithAnimation)
        {
            weaponData.Attack(cameraTransform, currentChargeAmount);
        }
    }

    public void AnimationAttack()
    {
        weaponData.Attack(cameraTransform, currentChargeAmount);
    }

    internal void EquipWeapon(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        HasSomethingEquiped = weaponData != null;
        if (HasSomethingEquiped) {
            weaponData.Equip(this);
        }
        currentAttackCooldown = 0f;
        CanAttack = true;
    }

    public void EquipMagicSpell(WeaponData weaponData)
    {
        weaponData.Equip(this);
    }


    private void Update()
    {
        if (!CanAttack)
        {
            if(currentAttackCooldown < 0)
            {
                CanAttack = true;
            }
            else
            {
                currentAttackCooldown -= Time.deltaTime;
            }
        }

        if (HasSomethingEquiped) {
            weaponData.Update();
        }
    }

    public void HitStop(float timeScale,float time)
    {
        Time.timeScale = timeScale;
        StartCoroutine(ResetHitStop(time));
    }

    IEnumerator ResetHitStop(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    internal void DamageDone(int amount)
    {
        playerResourceHandler.LifeSteal(amount, PlayerData.LifeStealPercentage);
    }
}
