using UnityEngine;


[CreateAssetMenu(fileName = "ExtraKillRegenerationBattleLoot", menuName = "BattleLoot/Buff/Epic/ExtraKillRegenerationBattleLoot", order = 1)]
public class ExtraKillRegenerationBattleLoot : BuffBattleLoot
{

    [SerializeField] private int extraKillRegenerationAmount = 1;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.ExtraManaKillAmount += 1;
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootRemoved(player, data);
        data.ExtraManaKillAmount -= 1;
    }

}
