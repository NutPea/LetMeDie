using UnityEngine;

[CreateAssetMenu(fileName = "LuckBuff", menuName = "BattleLoot/Buff/LuckBuff", order = 1)]
public class LuckBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private int luckBuff = 1;

    public override string Description => description + " " + luckBuff;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.Luck += luckBuff;
    }
}
