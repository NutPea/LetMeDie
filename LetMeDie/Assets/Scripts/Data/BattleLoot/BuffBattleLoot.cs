using UnityEngine;

public class BuffBattleLoot : BattleLoot
{
    protected PlayerData playerData;

    public virtual void BuffBattleLootAdded(GameObject player , PlayerData data)
    {
        this.playerData = data;
    }

    public virtual void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {

    }

}
