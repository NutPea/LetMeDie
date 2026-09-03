using UnityEngine;
[CreateAssetMenu(fileName = "ritChance", menuName = "BattleLoot/Buff/ritChance", order = 1)]
public class CritChanceBattleLoot : BuffBattleLoot
{
   
    [SerializeField] private float critChanceAmount = 0.05f;

    public override string Description => description + " " + critChanceAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.CritChance += critChanceAmount;
    }

}
