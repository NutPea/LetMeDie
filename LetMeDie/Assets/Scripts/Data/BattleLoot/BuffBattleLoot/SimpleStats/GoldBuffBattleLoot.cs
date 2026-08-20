using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;


[CreateAssetMenu(fileName = "GoldBuffBattleLoot", menuName = "BattleLoot/Buff/GoldBuffBattleLoot", order = 1)]
public class GoldBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float goldPercentage = 0.15f;

    public override string Description => description + " " + goldPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.GoldPercentage += goldPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.GoldPercentage -= goldPercentage;
    }
}
