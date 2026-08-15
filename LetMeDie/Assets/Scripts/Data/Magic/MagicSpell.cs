using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicSpell : WeaponData
{
    [SerializeField] private int spellManaCost = 10;
    public int SpellManaCost => spellManaCost;
    protected PlayerResourceHandler playerResourceHandler;
    public UnityEvent<MagicSpell> OnSpellCast = new();
    public List<InfluenceData> SpellInfluences = new();

    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        playerResourceHandler = playerWeaponController.GetComponent<PlayerResourceHandler>();
        foreach(InfluenceData influenceData in SpellInfluences)
        {
            influenceData.Init(playerWeaponController, this);
        }
    }

    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack(camera, chargeAmount);
        /*
        if (playerResourceHandler.CurrentMana >= SpellManaCost) {
            playerResourceHandler.UseMana(SpellManaCost);
            OnSpellCast.Invoke(this);
        }
        */

        Cast(camera);
        OnSpellCast.Invoke(this);
    }

    public virtual void Cast(Transform camera)
    {

    }

}
