using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StandingStillHealingBattleLoot", menuName = "BattleLoot/Buff/Rare/StandingStillHealingBattleLoot", order = 1)]
public class StandingStillHealingBattleLoot : BuffBattleLoot
{
    private HealthManager healthManager;
    private PlayerCharacterControllerMovementController movementController;
    [SerializeField] private float timeUntilHeal = 1f;
    private float currentTimeUntilHeal = 0f;
    [SerializeField] private float percentageHeal = 0.01f;


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        healthManager = player.GetComponent<HealthManager>();
        movementController = player.GetComponent<PlayerCharacterControllerMovementController>();
        movementController.OnStandingStill.AddListener(HealUpdate);

        currentTimeUntilHeal = timeUntilHeal;
    }

    private void HealUpdate()
    {
        if(currentTimeUntilHeal < 0f)
        {
            currentTimeUntilHeal = timeUntilHeal;
            float healAmount = healthManager.healthData.Health * percentageHeal;
            healthManager.Heal(Mathf.CeilToInt(healAmount));
        }
        else
        {
            currentTimeUntilHeal -= Time.deltaTime;
        }
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootRemoved(player, data);
    }


}
