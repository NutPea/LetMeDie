using UnityEngine;

public class InfluenceData : ScriptableObject
{

    [SerializeField] private string description;
    public string Description => description;

    public float commonUpgradePercentage = 0.02f;
    public float uncommonUpgradePercentage = 0.05f;
    public float rareUpgradePercentage = 0.1f;
    public float epicUpgradePercentage = 0.15f;
    public float legendaryUpgradePercentage = 0.2f;

    protected float currentUpgradeAmount = 0;

    public virtual void Init(PlayerWeaponController playerWeaponController,MagicSpell magicSpell)
    {

    }

    public virtual void CalculateSpellUpgrade(MagicSpell spell, BattleLoot.LootRarity rarity)
    {

    }

    public virtual void UpgradeSpell(MagicSpell spell)
    {

    }


    public virtual void OnSpellCast(GameObject spawnedSpell)
    {

    }

    public virtual string UpgradeText() {
        return "";
    }

    protected virtual float GetPercentage(BattleLoot.LootRarity rarity)
    {
        switch (rarity)
        {
            case BattleLoot.LootRarity.Common:return commonUpgradePercentage;
            case BattleLoot.LootRarity.Uncommen:return uncommonUpgradePercentage;
            case BattleLoot.LootRarity.Rare: return rareUpgradePercentage;
            case BattleLoot.LootRarity.Epic: return epicUpgradePercentage;
            case BattleLoot.LootRarity.Legendary: return legendaryUpgradePercentage;
        }
        return 0;

    }

}
