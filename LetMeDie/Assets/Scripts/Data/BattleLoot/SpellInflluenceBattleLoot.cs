using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellInflluenceBattleLoot : BattleLoot
{
    private MagicSpell spell;
    public override string Name => spell.ItemName;
    public override string Description {

        get {
            string influenceDescription = "";
            foreach(InfluenceData influenceData in spellInfluences)
            {
                influenceDescription += influenceData.UpgradeText();
            }       
            return influenceDescription;
        }
    }
    public override Sprite Icon => spell.Sprite;
    public override Color Tint => spell.Tint;

    private List<InfluenceData> spellInfluences = new();
  


    public void SetSpell(MagicSpell magicSpell,LootRarity lootRarity)
    {
        this.lootRarity = lootRarity;
        spell = magicSpell;
        float commonPercentage = 0.0f;
        float uncommonPercentage = 0.0f;
        float rarePercentage = 0.0f;
        float epicPercentage = 0.0f;
        float legendaryPercentage = 0.0f;
        (commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage) = SpellLevelUpUIState.GetDropPercentage(lootRarity);

        int amountOfUpgrades = AmountOfUpgrades(lootRarity);
        if (amountOfUpgrades > magicSpell.SpellInfluences.Count) {
            magicSpell.SpellInfluences.ForEach((data) => spellInfluences.Add(data));
        }
        else
        {
            List<InfluenceData> availableDatas = new(); 
            magicSpell.SpellInfluences.ForEach((data) => availableDatas.Add(data));
            for (int i = 0; i < amountOfUpgrades; i++) {
                InfluenceData data = availableDatas[UnityEngine.Random.Range(0, availableDatas.Count)];
                spellInfluences.Add(data);
                data.CalculateSpellUpgrade(magicSpell, SpellLevelUpUIState.GetRarity(commonPercentage, uncommonPercentage, rarePercentage, epicPercentage, legendaryPercentage));
                availableDatas.Remove(data);
            }
        }
    }

    public void UpgradeSpell()
    {
        foreach(InfluenceData data in spellInfluences)
        {
            data.UpgradeSpell(spell);
        }
    }

    private int AmountOfUpgrades(LootRarity lootRarity)
    {
        switch (lootRarity) {
            case LootRarity.Common: return 1;
            case LootRarity.Uncommen: return 1;
            case LootRarity.Rare: return UnityEngine.Random.Range(1,2);
            case LootRarity.Epic: return 2;
            case LootRarity.Legendary: return UnityEngine.Random.Range(2,3);

            default: return 1;
        }
    }




}
