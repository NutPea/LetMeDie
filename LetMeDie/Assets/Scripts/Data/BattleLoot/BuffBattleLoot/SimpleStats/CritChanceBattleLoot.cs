using UnityEngine;
[CreateAssetMenu(fileName = "ritChance", menuName = "BattleLoot/Buff/CritChance", order = 1)]
public class CritChanceBattleLoot : BuffBattleLoot
{
   
    [SerializeField] private float critChanceAmount = 0.05f;
    private float CritChanceAmount => critChanceAmount * CurrentRarityModifier;

    public override string Description => description + " " + CritChanceAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.CritChance += CritChanceAmount;
    }

}
