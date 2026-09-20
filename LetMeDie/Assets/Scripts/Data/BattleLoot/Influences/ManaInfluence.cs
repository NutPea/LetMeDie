using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Influence/Mana", order = 1)]
public class ManaInfluence : SpellInfluenceData
{
    int beforeManaValue = 0;
    int afterManaValue = 0;
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
            int toCalValue = 0;
            if(projectileSpell.ExtraManaRegOnKill == 0)
            {
                toCalValue = 1;
            }
            else
            {
                toCalValue = projectileSpell.ExtraManaRegOnKill;
            }
            beforeManaValue = projectileSpell.ExtraManaRegOnKill;
            float potentialUpgradeAmount = GetPercentage(rarity) + currentUpgradeAmount;
            lastrarity = rarity;
            beforeManaValue = Mathf.CeilToInt(currentUpgradeAmount);
            afterManaValue = Mathf.CeilToInt(potentialUpgradeAmount);
        }
    }

    public override void UpgradeSpell(MagicSpell spell)
    {
        base.UpgradeSpell(spell);
        if (spell is MagicProjectileSpell projectileSpell)
        {


            currentUpgradeAmount += GetPercentage(lastrarity);
            projectileSpell.ExtraManaRegOnKill = afterManaValue;
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
            return $"{Description} : {currentProjectileSpell.ExtraManaRegOnKill + beforeManaValue} > {currentProjectileSpell.ExtraManaRegOnKill + afterManaValue} ";
        }
        return $"{Description} : {beforeManaValue} > {afterManaValue} ";
    }
}
