using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegBattleLoot", menuName = "BattleLoot/Buff/HealthRegBattleLoot", order = 1)]
public class HealthRegBattleLoot : BuffBattleLoot
{
    [SerializeField] private float healthRegAmount = 0.05f;

    public override string Description => description + " " + healthRegAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.HealthRegRate += healthRegAmount;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.HealthRegRate -= healthRegAmount;
    }

}
