using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GoldReceiveByHitBattleLoot", menuName = "BattleLoot/Buff/Uncommon/GoldReceiveByHitBattleLoot", order = 1)]
public class GoldReceiveByHitBattleLoot : BuffBattleLoot
{

    [SerializeField] private int amountOfGold = 5;
    public override string Description => description + " " + amountOfGold + " Gold";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        HealthManager healthManager = player.GetComponent<HealthManager>();
        healthManager.OnDamaged.AddListener(OnReceiveGold);
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        HealthManager healthManager = player.GetComponent<HealthManager>();
        healthManager.OnDamaged.RemoveListener(OnReceiveGold);
    }

    private void OnReceiveGold(bool arg0, int arg1, Transform arg2)
    {
        playerData.AddGold(amountOfGold);
    }
}
