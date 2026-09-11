using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;

[CreateAssetMenu(fileName = "HPBuffBattleLoot", menuName = "BattleLoot/Buff/Character/HPBuffBattleLoot", order = 1)]
public class HPBuffBattleLoot : BuffBattleLoot
{
    [Header("Stats")]

    [SerializeField] private int bonusHP = 25;
    private int BonusHP => bonusHP + (Mathf.CeilToInt(bonusHP * CurrentRarityModifier)); 
    public override string Description => description + " " + bonusHP;


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraHealth += BonusHP;
        player.GetComponent<PlayerResourceHandler>().Heal(bonusHP);
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraHealth -= BonusHP;
    }


    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.Health);
        afterUpgradeValue = CalculateString(playerData.Health + BonusHP);
    }
}
