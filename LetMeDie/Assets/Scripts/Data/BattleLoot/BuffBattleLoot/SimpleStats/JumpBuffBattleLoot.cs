using UnityEngine;

[CreateAssetMenu(fileName = "JumpBuffBattleLoot", menuName = "BattleLoot/Buff/JumpBuff", order = 1)]
public class JumpBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float jumpPercentage = 0.15f;

    public override string Description => description + " " + jumpPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraJumpSpeedPercent += jumpPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraJumpSpeedPercent -= jumpPercentage;
    }

}
