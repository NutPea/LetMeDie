using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDamageBuffBattleLoot", menuName = "BattleLoot/Buff/WeaponDamageBuff", order = 1)]
public class WeaponDamageBuffBattleLoot : BuffBattleLoot
{
    [Header("Stats")]

    [SerializeField] private float damagePercentage = 0.15f;
    [SerializeField] private float DamagePercentage => damagePercentage + (damagePercentage * CurrentRarityModifier);

    public override string Description => description + " " + damagePercentage * 100f + "%";


    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.WeaponBaseDamagePercentage);
        afterUpgradeValue = CalculateString(playerData.WeaponBaseDamagePercentage + DamagePercentage);
    }

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponBaseDamagePercentage += DamagePercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponBaseDamagePercentage -= DamagePercentage;
    }


}
