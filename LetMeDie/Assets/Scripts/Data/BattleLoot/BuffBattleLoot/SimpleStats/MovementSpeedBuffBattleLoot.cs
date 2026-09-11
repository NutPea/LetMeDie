using UnityEngine;

[CreateAssetMenu(fileName = "MovementSpeedBuffBattleLoot", menuName = "BattleLoot/Buff/MovementSpeedBuff", order = 1)]
public class MovementSpeedBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float speedAddPercentage = 0.15f;
    private float SpeedAddPercentage => speedAddPercentage + (speedAddPercentage * CurrentRarityModifier);
    public override string Description => description + " " + speedAddPercentage * 100f + "%";



    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.ExtraMovementSpeedPercent);
        afterUpgradeValue = CalculateString(playerData.ExtraMovementSpeedPercent + SpeedAddPercentage);
    }

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
