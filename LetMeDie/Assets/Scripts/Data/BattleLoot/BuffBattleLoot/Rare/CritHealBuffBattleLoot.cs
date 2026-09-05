using System;
using UnityEngine;

[CreateAssetMenu(fileName = " CritHealBuff", menuName = "BattleLoot/Buff/Rare/CritHealBuff", order = 1)]
public class CritHealBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private int healAmount = 5;
    [SerializeField] private float healCritPropability = 0.2f;

    public override string Description => $" Whenever you crit you have a {healCritPropability * 100}% Chance to heal for {healAmount}";
    private HealthManager playerHealthManager;


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerHealthManager = player.GetComponent<HealthManager>();
        data.OnCrit.AddListener(PlayerHasCrit);

    }

    private void PlayerHasCrit()
    {
        float percentage = UnityEngine.Random.Range(0.0f, 1.0f);
        if(percentage < healCritPropability)
        {
            playerHealthManager.Heal(healAmount);
        }


    }
}
