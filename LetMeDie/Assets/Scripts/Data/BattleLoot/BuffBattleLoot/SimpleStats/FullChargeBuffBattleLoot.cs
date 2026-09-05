using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegBattleLoot", menuName = "BattleLoot/Buff/FullChargeBuffBattleLoot", order = 1)]
public class FullChargeBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float fullChargeDamageAmount = 0.05f;

    public override string Description => description + " " + fullChargeDamageAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponExtraChargeDamage += fullChargeDamageAmount;
    }

}
