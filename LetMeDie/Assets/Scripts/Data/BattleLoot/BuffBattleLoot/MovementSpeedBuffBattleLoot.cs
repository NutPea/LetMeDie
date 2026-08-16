using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedBuffBattleLoot", menuName = "BattleLoot/Buff/MovementSpeedBuff", order = 1)]
public class MovementSpeedBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float SpeedAddPercentage = 0.15f;

    public override string Description => description + " " + SpeedAddPercentage;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraMovementSpeedPercent += SpeedAddPercentage;    
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraMovementSpeedPercent -= SpeedAddPercentage;
    }

}
