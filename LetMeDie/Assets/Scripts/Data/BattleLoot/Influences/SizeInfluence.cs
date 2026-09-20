using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Influence/Size", order = 1)]
public class SizeInfluence : SpellInfluenceData
{
    float beforeSizeValue = 0;
    float afterSizeValue = 0;
    private BattleLoot.LootRarity lastrarity;
    private MagicProjectileSpell currentProjectileSpell;


    public override void Init(PlayerWeaponController playerWeaponController, MagicSpell magicSpell)
    {
        base.Init(playerWeaponController, magicSpell);
        if (magicSpell is MagicProjectileSpell projectileSpell)
        {
            currentProjectileSpell = projectileSpell;
        }
    }

    public override void CalculateSpellUpgrade(MagicSpell spell, BattleLoot.LootRarity rarity)
    {
        base.CalculateSpellUpgrade(spell, rarity);
        if (spell is MagicProjectileSpell projectileSpell)
        {
            float toCalValue = 0;
            if (projectileSpell.ExtraSize == 0)
            {
                toCalValue = .25f;
            }
            else
            {
                toCalValue = projectileSpell.ExtraSize;
            }


            beforeSizeValue = projectileSpell.ExtraSize;
            float potentialUpgradeAmount = GetPercentage(rarity) + currentUpgradeAmount;
            lastrarity = rarity;
            beforeSizeValue =  currentUpgradeAmount;
            afterSizeValue = potentialUpgradeAmount;
        }
    }

    public override void UpgradeSpell(MagicSpell spell)
    {
        base.UpgradeSpell(spell);
        if (spell is MagicProjectileSpell projectileSpell)
        {
            currentUpgradeAmount += GetPercentage(lastrarity);
            projectileSpell.ExtraSize = afterSizeValue;
        }
    }

    public override void OnSpellCast(GameObject spawnedSpell)
    {
        base.OnSpellCast(spawnedSpell);
    }

    public override string UpgradeText()
    {
        string beforeValue = ((currentProjectileSpell.ExtraSize + beforeSizeValue)).ToString("F2");
        string afterValue = ((currentProjectileSpell.ExtraSize + afterSizeValue)).ToString("F2");


        return $"{Description} " + beforeValue + " > " + afterValue;
    }
}
