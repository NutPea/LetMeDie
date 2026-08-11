using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ItemData
{

    [SerializeField] protected int minDamageAmount;
    [SerializeField] protected int maxDamageAmount;
    [SerializeField] protected bool canBlock;
    public bool CanBlock => canBlock;
    [SerializeField] private float fullChargeTime = 0.5f;
    public float FullChargeTime => fullChargeTime;

    protected PlayerData playerData;
    protected PlayerWeaponController playerWeaponController;
    protected int attackLayer;

    [SerializeField] private float attackCooldown = 0.5f;
    public float AttackCooldown => attackCooldown;
    [SerializeField] private List<CombatEffect> combatEffect;

    private List<CombatEffect> instanciatedCombatEffects;

    public List<CombatEffect> CombatEffects => instanciatedCombatEffects;

    public virtual void Equip(PlayerWeaponController playerWeaponController)
    {
        attackLayer = ~LayerMask.GetMask("Player");
        playerData = playerWeaponController.PlayerData;
        instanciatedCombatEffects.Clear();
        foreach (CombatEffect combatEffect in combatEffect) {
            CombatEffect effect = Instantiate(combatEffect);
            effect.Init(playerWeaponController.transform);
            instanciatedCombatEffects.Add(effect);
        }
        this.playerWeaponController = playerWeaponController;
    }

    public virtual void Attack(Transform camera,float chargeAmount)
    {

    }

    public virtual void Update()
    {

    }

}
