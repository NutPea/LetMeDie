using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


[CreateAssetMenu(fileName = "ExtraChargeSpeedPercentageBuff", menuName = "BattleLoot/Buff/ExtraChargeSpeedPercentageBuff", order = 1)]
public class ExtraChargeSpeedPercentageBuffBattleLoot : BuffBattleLoot
{
    [Header("Stats")]
    [SerializeField] private float extraChargeSpeed = 0.05f;
    private float ExtraChargeSpeed => extraChargeSpeed + (extraChargeSpeed * CurrentRarityModifier);

    public override string Description => description + " " + ExtraChargeSpeed * 100f + "%";

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.ExtraChargeSpeedPercentage);
        afterUpgradeValue = CalculateString(playerData.ExtraChargeSpeedPercentage + ExtraChargeSpeed);
    }


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraChargeSpeedPercentage += ExtraChargeSpeed;
    }
}
