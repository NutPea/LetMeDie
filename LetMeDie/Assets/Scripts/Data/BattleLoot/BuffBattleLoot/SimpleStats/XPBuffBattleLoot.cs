using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;


[CreateAssetMenu(fileName = "XPBuffBattleLoot", menuName = "BattleLoot/Buff/Character/XPBuffBattleLoot", order = 1)]
public class XPBuffBattleLoot : BuffBattleLoot
{

    [Header("Stats")]
    [SerializeField] private float xpPercentage = 0.15f;
    private float XpPercentage => xpPercentage + (xpPercentage * CurrentRarityModifier);
    public override string Description => description + " " + xpPercentage * 100f + "%";

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.ExpGainPercentage);
        afterUpgradeValue = CalculateString(playerData.ExpGainPercentage + XpPercentage);
    }

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpGainPercentage += XpPercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExpGainPercentage -= XpPercentage;
    }
}
