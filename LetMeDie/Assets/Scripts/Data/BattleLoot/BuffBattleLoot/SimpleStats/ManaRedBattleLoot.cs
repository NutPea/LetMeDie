using UnityEngine;

[CreateAssetMenu(fileName = "ManaReductionBattleLoot", menuName = "BattleLoot/Buff/ManaReductionBattleLoot", order = 1)]
public class ManaRedBattleLoot : BuffBattleLoot
{
    [SerializeField] private float manaReductionValue = 0.05f;
    protected float ManaReductionValue => manaReductionValue * CurrentRarityModifier;

    public override string Description => description + " " + ManaReductionValue * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaReduction += ManaReductionValue;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaReduction -= ManaReductionValue;
    }
}
