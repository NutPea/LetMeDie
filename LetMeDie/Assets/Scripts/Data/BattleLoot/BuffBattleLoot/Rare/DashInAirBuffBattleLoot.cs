using UnityEngine;

[CreateAssetMenu(fileName = " CritHealBuff", menuName = "BattleLoot/Buff/Rare/DashInAir", order = 1)]
public class DashInAirBuffBattleLoot : BuffBattleLoot
{
    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        player.GetComponent<PlayerCharacterControllerMovementController>().NeedsToBeGroundedToDash = false;

    }
}
