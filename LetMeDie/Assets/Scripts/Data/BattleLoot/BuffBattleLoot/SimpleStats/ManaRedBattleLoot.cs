using UnityEngine;

[CreateAssetMenu(fileName = "ManaReductionBattleLoot", menuName = "BattleLoot/Buff/ManaReductionBattleLoot", order = 1)]
public class ManaRedBattleLoot : BuffBattleLoot
{
    [SerializeField] private float manaReductionValue = 0.05f;

    public override string Description => description + " " + manaReductionValue*100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaReduction += manaReductionValue;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaReduction -= manaReductionValue;
    }
}
