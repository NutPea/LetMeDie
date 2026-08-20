using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDamageBuffBattleLoot", menuName = "BattleLoot/Buff/WeaponDamageBuff", order = 1)]
public class WeaponDamageBuffBattleLoot : BuffBattleLoot
{

    [SerializeField] private float damagePercentage = 0.15f;

    public override string Description => description + " " + damagePercentage * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponBaseDamagePercentage += damagePercentage;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.WeaponBaseDamagePercentage -= damagePercentage;
    }


}
