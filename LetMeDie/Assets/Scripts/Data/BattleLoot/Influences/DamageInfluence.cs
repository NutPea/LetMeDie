using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Influence/Damage", order = 1)]
public class DamageInfluence : InfluenceData
{
    int beforeDamageValue;
    int afterDamageValue;
    private BattleLoot.LootRarity lastrarity;


    public override void CalculateSpellUpgrade(MagicSpell spell, BattleLoot.LootRarity rarity)
    {
        base.CalculateSpellUpgrade(spell, rarity);
        if (spell is MagicProjectileSpell projectileSpell)
        {
            beforeDamageValue = projectileSpell.Damage;
            float potentialUpgradeAmount = GetPercentage(rarity) + currentUpgradeAmount;
            lastrarity = rarity;
            beforeDamageValue = Mathf.CeilToInt((float)projectileSpell.BaseDamage * currentUpgradeAmount);
            afterDamageValue = Mathf.CeilToInt((float)projectileSpell.BaseDamage * potentialUpgradeAmount);
        }
    }

    public override void UpgradeSpell(MagicSpell spell)
    {
        base.UpgradeSpell(spell);
        if(spell is MagicProjectileSpell projectileSpell)
        {
            currentUpgradeAmount += GetPercentage(lastrarity);
            projectileSpell.ExtraDamage = afterDamageValue;
        }
    }

    public override void OnSpellCast(GameObject spawnedSpell)
    {
        base.OnSpellCast(spawnedSpell);
    }

    public override string UpgradeText()
    {
        return $"{Description} : {beforeDamageValue} > {afterDamageValue} ";
    }

}
