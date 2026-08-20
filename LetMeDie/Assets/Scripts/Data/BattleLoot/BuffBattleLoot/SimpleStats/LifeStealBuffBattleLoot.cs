using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;

[CreateAssetMenu(fileName = "LifeStealBuffBattleLoot", menuName = "BattleLoot/Buff/LifeStealBuffBattleLoot", order = 1)]
public class LifeStealBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float lifeStealPercentage = 0.05f;

    public override string Description => description + " " + lifeStealPercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.LifeStealPercentage += lifeStealPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.LifeStealPercentage -= lifeStealPercentage;
    }
}
