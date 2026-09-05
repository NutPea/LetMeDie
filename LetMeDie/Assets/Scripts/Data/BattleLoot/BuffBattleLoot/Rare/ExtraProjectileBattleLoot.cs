using UnityEngine;

[CreateAssetMenu(fileName = "ExtraProjectileBattleLoot", menuName = "BattleLoot/Buff/Rare/ExtraProjectileBattleLoot", order = 1)]
public class ExtraProjectileBattleLoot : BuffBattleLoot
{

    [SerializeField] public int extraProjectiles;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.ExtraAmountOfProjectiles += extraProjectiles;
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootRemoved(player, data);
        data.ExtraAmountOfProjectiles -= extraProjectiles;
    }

}
