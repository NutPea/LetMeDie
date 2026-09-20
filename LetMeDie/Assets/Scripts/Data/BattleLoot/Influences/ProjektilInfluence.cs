using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Influence/Projectile", order = 1)]
public class ProjektilInfluence : SpellInfluenceData
{
    int beforeProjectileValue = 0;
    int afterProjectileValue = 0;
    private BattleLoot.LootRarity lastrarity;
    private SpreadMagicProjectileSpell currentProjectileSpell;


    public override void Init(PlayerWeaponController playerWeaponController, MagicSpell magicSpell)
    {
        base.Init(playerWeaponController, magicSpell);
        if (magicSpell is SpreadMagicProjectileSpell projectileSpell)
        {
            currentProjectileSpell = projectileSpell;
        }
    }

    public override void CalculateSpellUpgrade(MagicSpell spell, BattleLoot.LootRarity rarity)
    {
        base.CalculateSpellUpgrade(spell, rarity);
        if (spell is SpreadMagicProjectileSpell projectileSpell)
        {
            int toCalValue = 0;
            if (projectileSpell.ExtraAmountOfProjectiles == 0)
            {
                toCalValue = 1;
            }
            else
            {
                toCalValue = projectileSpell.ExtraAmountOfProjectiles;
            }

            beforeProjectileValue = Mathf.CeilToInt(currentUpgradeAmount);
            float potentialUpgradeAmount = GetPercentage(rarity) + currentUpgradeAmount;
            lastrarity = rarity;
            afterProjectileValue = Mathf.CeilToInt(potentialUpgradeAmount);
        }
    }

    public override void UpgradeSpell(MagicSpell spell)
    {
        base.UpgradeSpell(spell);
        if (spell is SpreadMagicProjectileSpell projectileSpell)
        {
            currentUpgradeAmount += GetPercentage(lastrarity);
            projectileSpell.ExtraAmountOfProjectiles = afterProjectileValue;
        }
    }

    public override void OnSpellCast(GameObject spawnedSpell)
    {
        base.OnSpellCast(spawnedSpell);
    }

    public override string UpgradeText()
    {
        if (currentProjectileSpell != null)
        {
            return $"{Description} : {currentProjectileSpell.ExtraAmountOfProjectiles + beforeProjectileValue} > {currentProjectileSpell.ExtraAmountOfProjectiles + afterProjectileValue} ";
        }
        return $"{Description} : {beforeProjectileValue} > {afterProjectileValue} ";
    }
}
