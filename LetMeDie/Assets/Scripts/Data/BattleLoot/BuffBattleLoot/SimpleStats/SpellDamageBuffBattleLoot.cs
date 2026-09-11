using UnityEngine;


[CreateAssetMenu(fileName = "SpellDamageBuffBattleLoot", menuName = "BattleLoot/Buff/SpellDamageBuff", order = 1)]
public class SpellDamageBuffBattleLoot : BuffBattleLoot
{
    [Header("Stats")]

    [SerializeField] private float spellDamagePercentage = 0.1f;
    private float SpellDamagePercentage => spellDamagePercentage + (spellDamagePercentage * CurrentRarityModifier);

    public override string Description => description + " " + spellDamagePercentage * 100f + "%";



    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.SpellBaseDamagePercentage);
        afterUpgradeValue = CalculateString(playerData.SpellBaseDamagePercentage + SpellDamagePercentage);
    }


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellBaseDamagePercentage += SpellDamagePercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellBaseDamagePercentage -= SpellDamagePercentage;
    }
}
