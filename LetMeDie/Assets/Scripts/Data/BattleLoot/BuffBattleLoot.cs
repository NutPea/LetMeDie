using UnityEngine;

public class BuffBattleLoot : BattleLoot
{
    protected PlayerData playerData;

    [SerializeField] private bool showsAsItem = true;
    public bool ShowsAsItem => showsAsItem;

    public virtual void BuffBattleLootAdded(GameObject player , PlayerData data)
    {
        this.playerData = data;
    }

    public virtual void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {

    }

}
