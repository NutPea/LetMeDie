using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;


[CreateAssetMenu(fileName = "XPBuffBattleLoot", menuName = "BattleLoot/Buff/XPBuffBattleLoot", order = 1)]
public class XPBuffBattleLoot : BuffBattleLoot
{

    [SerializeField] private float xpPercentage = 0.15f;

    public override string Description => description + " " + xpPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpGainPercentage += xpPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpGainPercentage -= xpPercentage;
    }
}
