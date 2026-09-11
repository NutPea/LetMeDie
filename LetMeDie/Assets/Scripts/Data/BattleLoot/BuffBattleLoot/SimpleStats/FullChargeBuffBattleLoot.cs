using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegBattleLoot", menuName = "BattleLoot/Buff/FullChargeBuffBattleLoot", order = 1)]
public class FullChargeBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float fullChargeDamageAmount = 0.05f;
    private float FullChargeDamageAmount => fullChargeDamageAmount + (fullChargeDamageAmount * CurrentRarityModifier);
    public override string Description => description + " " + fullChargeDamageAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponExtraChargeDamage += fullChargeDamageAmount;
    }

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.WeaponExtraChargeDamage);
        afterUpgradeValue = CalculateString(playerData.WeaponExtraChargeDamage + FullChargeDamageAmount);
    }

}
