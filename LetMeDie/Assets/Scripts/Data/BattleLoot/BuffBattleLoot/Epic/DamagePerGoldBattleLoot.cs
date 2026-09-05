using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamagePerGoldBattleLoot", menuName = "BattleLoot/Buff/Epic/DamagePerGoldBattleLoot", order = 1)]
public class DamagePerGoldBattleLoot : BuffBattleLoot
{
    [SerializeField] private float maxDamageIncrease = 1f;
    [SerializeField] private float damageIncresePerGoldStack = 0.01f;
    [SerializeField] private int goldStackAmount = 100;
    private float lastIncrease = 0.0f;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.OnGoldChange.AddListener(OnGoldUpdate);
    }

    private void OnGoldUpdate(int amount)
    {
        playerData.WeaponBaseDamagePercentage -= lastIncrease;
        playerData.SpellBaseDamagePercentage += lastIncrease;
        float percentageAmountOfGold = amount / goldStackAmount;
        float damageIncrease = percentageAmountOfGold * damageIncresePerGoldStack;
        if (damageIncrease > maxDamageIncrease) {
            damageIncrease = maxDamageIncrease;
        }
        playerData.WeaponBaseDamagePercentage += damageIncrease;
        playerData.SpellBaseDamagePercentage += damageIncrease;
        lastIncrease = damageIncrease;
    }
}
