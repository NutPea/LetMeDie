using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HPGainByKillBuffBattleLoot", menuName = "BattleLoot/Buff/Uncommon/HPGainByKillBuffBattleLoot", order = 1)]
public class HPGainByKillBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private int maxHPGain = 100;
    [SerializeField] private float hpGainPerKill;
    int killedEnemys;
    float currentHPGGain;
    int alreadyGivenHP = 0;

    public override string Description => $"For every kill you get {hpGainPerKill} to a maximum of {maxHPGain}";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        SGameManager.Instance.OnEnemyKilled.AddListener(OnEnemyKilled);
    }

    private void OnEnemyKilled(int arg0)
    {
        if(alreadyGivenHP >= maxHPGain)
        {
            return;
        }

        killedEnemys++;
        currentHPGGain = hpGainPerKill * killedEnemys;

        float difference = currentHPGGain - alreadyGivenHP;
        if(difference > 1)
        {
            int giveHp = Mathf.CeilToInt(difference);
            alreadyGivenHP += giveHp;
            playerData.ExtraHealth += giveHp;
            playerData.OnStatUpdate.Invoke();
        }

        
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        SGameManager.Instance.OnEnemyKilled.RemoveListener(OnEnemyKilled);
    }

  
}
