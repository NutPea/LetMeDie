using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicSpell : WeaponData
{
    [SerializeField] private int spellManaCost = 10;
    public int SpellManaCost => spellManaCost - Mathf.CeilToInt((float)spellManaCost * playerData.SpellManaReduction);

    private int extraManaRegOnKill = 0;
    public int ExtraManaRegOnKill { get => extraManaRegOnKill; set => extraManaRegOnKill = value; }

    private int currentSpellMana = 0;
    public int CurrentSpellMana => currentSpellMana;
    private bool SpellIsReady => currentSpellMana >= SpellManaCost;

    protected PlayerResourceHandler playerResourceHandler;
    public UnityEvent<MagicSpell> OnSpellCast = new();
    [SerializeField] private List<SpellInfluenceData> spellInfluences = new();
    public List<SpellInfluenceData> SpellInfluences => copiedSpellInfluences;
    private List<SpellInfluenceData> copiedSpellInfluences = new();

    [HideInInspector] public UnityEvent<int,int> OnSpellAmountUpdate = new();
    private Transform mainCamera;

    public override void Equip(PlayerWeaponController playerWeaponController)
    {
        base.Equip(playerWeaponController);
        playerResourceHandler = playerWeaponController.GetComponent<PlayerResourceHandler>();
        copiedSpellInfluences.Clear();
        foreach (SpellInfluenceData influenceData in spellInfluences)
        {
            SpellInfluenceData data = Instantiate(influenceData);
            data.Init(playerWeaponController, this);
            copiedSpellInfluences.Add(data);
        }
        currentSpellMana = SpellManaCost;
        extraManaRegOnKill = 0;

    }

    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack(camera, chargeAmount);
        mainCamera = camera;
        if (SpellIsReady) {
            CastAttack();

            currentSpellMana = 0;
            OnSpellAmountUpdate.Invoke(currentSpellMana, SpellManaCost);


            playerWeaponController.CastExtraAttack(() => CastAttack(), playerData.AmountOfExtraCasts);

        }
    }

    private void CastAttack()
    {
        Cast(mainCamera);
        OnSpellCast.Invoke(this);
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
        currentSpellMana += playerData.ExtraManaKillAmount + 1 + ExtraManaRegOnKill;
        if (currentSpellMana > SpellManaCost) {
            currentSpellMana = SpellManaCost;
        }
    }

}
