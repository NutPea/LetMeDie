using UnityEngine;

[CreateAssetMenu(fileName = "LuckBuff", menuName = "BattleLoot/Buff/LuckBuff", order = 1)]
public class LuckBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private int luckBuff = 1;
    private int LuckBuff => luckBuff + Mathf.CeilToInt(luckBuff * CurrentRarityModifier);

    public override string Description => description + " " + luckBuff;



    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.Luck);
        afterUpgradeValue = CalculateString(playerData.Luck + LuckBuff);
    }

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.Luck += LuckBuff;
    }
}
