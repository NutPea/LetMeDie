using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedBuffBattleLoot", menuName = "BattleLoot/Buff/MovementSpeedBuff", order = 1)]
public class MovementSpeedBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float speedAddPercentage = 0.15f;

    public override string Description => description + " " + speedAddPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraMovementSpeedPercent += speedAddPercentage;    
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraMovementSpeedPercent -= speedAddPercentage;
    }

}
