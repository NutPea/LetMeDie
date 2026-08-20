using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;

[CreateAssetMenu(fileName = "HPBuffBattleLoot", menuName = "BattleLoot/Buff/HPBuffBattleLoot", order = 1)]
public class HPBuffBattleLoot : BuffBattleLoot
{

    [SerializeField] private int bonusHP = 25;

    public override string Description => description + " " + bonusHP;


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraHealth += bonusHP;
        player.GetComponent<PlayerResourceHandler>().Heal(bonusHP);
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraHealth -= bonusHP;
    }

}
