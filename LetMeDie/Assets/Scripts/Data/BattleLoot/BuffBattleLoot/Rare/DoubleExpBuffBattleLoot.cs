using UnityEngine;


[CreateAssetMenu(fileName = "DoubleExpBuffBattleLoot", menuName = "BattleLoot/Buff/Rare/DoubleExpBuffBattleLoot", order = 1)]
public class DoubleExpBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float doubleExpPercentage = 0.1f;

    public override string Description => description + " " + doubleExpPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpDoubleChance += doubleExpPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpDoubleChance -= doubleExpPercentage;
    }


}
