using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ShrineHealBatlleLoot", menuName = "BattleLoot/Buff/Rare/ShrineHealBatlleLoot", order = 1)]
public class ShrineHealBatlleLoot : BuffBattleLoot
{

    [SerializeField] public float healPercentAmount;
    private HealthManager healthManager;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        healthManager = player.GetComponent<HealthManager>();
        data.OnSpellShrineUpgrade.AddListener(HealPlayer);
    }

    private void HealPlayer()
    {
        float healAmount = healthManager.healthData.Health * healPercentAmount;
        healthManager.Heal(Mathf.CeilToInt(healAmount));
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootRemoved(player, data);
        healthManager = player.GetComponent<HealthManager>();
        data.OnSpellShrineUpgrade.RemoveListener(HealPlayer);
    }


}
