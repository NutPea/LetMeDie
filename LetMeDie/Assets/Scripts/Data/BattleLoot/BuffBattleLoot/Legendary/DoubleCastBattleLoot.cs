using UnityEngine;

[CreateAssetMenu(fileName = "DoubleCastBattleLoot", menuName = "BattleLoot/Buff/Legendary/DoubleCastBattleLoot", order = 1)]
public class DoubleCastBattleLoot : BuffBattleLoot
{
    [SerializeField] private int extraCastAmount = 1;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.AmountOfExtraCasts += extraCastAmount;
    }

    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootRemoved(player, data);
        data.AmountOfExtraCasts -= extraCastAmount;
    }



}
