using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicSpell : WeaponData
{
    [SerializeField] private int spellManaCost = 10;
    public int SpellManaCost => spellManaCost - Mathf.CeilToInt((float)spellManaCost * playerData.SpellManaReduction);

    private int currentSpellMana = 0;
    private bool SpellIsReady => currentSpellMana >= SpellManaCost;

    protected PlayerResourceHandler playerResourceHandler;
    public UnityEvent<MagicSpell> OnSpellCast = new();
    [SerializeField] private List<InfluenceData> spellInfluences = new();
    public List<InfluenceData> SpellInfluences => copiedSpellInfluences;
    private List<InfluenceData> copiedSpellInfluences = new();

    [HideInInspector] public UnityEvent<int,int> OnSpellAmountUpdate = new();

    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        playerResourceHandler = playerWeaponController.GetComponent<PlayerResourceHandler>();
        copiedSpellInfluences.Clear();
        foreach (InfluenceData influenceData in spellInfluences)
        {
            InfluenceData data = Instantiate(influenceData);
            data.Init(playerWeaponController, this);
            copiedSpellInfluences.Add(data);
        }
        currentSpellMana = SpellManaCost;
    }

    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack(camera, chargeAmount);
        
        if (SpellIsReady) {
            Cast(camera);
            OnSpellCast.Invoke(this);
            currentSpellMana = 0;
            OnSpellAmountUpdate.Invoke(currentSpellMana, SpellManaCost);
        }
    }

    public virtual void Cast(Transform camera)
    {

    }

    public void RegMana()
    {
        if(currentSpellMana < SpellManaCost)
        {
            AddMana();
            OnSpellAmountUpdate.Invoke(currentSpellMana , SpellManaCost);
        }
    }

    public void AddMana()
    {
        currentSpellMana++;
        if (currentSpellMana > SpellManaCost) {
            currentSpellMana = SpellManaCost;
        }
    }

}
