using UnityEngine;


[CreateAssetMenu(fileName = "SpellDamageBuffBattleLoot", menuName = "BattleLoot/Buff/SpellDamageBuff", order = 1)]
public class SpellDamageBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float spellDamagePercentage = 0.1f;

    public override string Description => description + " " + spellDamagePercentage;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellBaseDamagePercentage += spellDamagePercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellBaseDamagePercentage += spellDamagePercentage;
    }
}
