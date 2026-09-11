using UnityEngine;
[CreateAssetMenu(fileName = "ritChance", menuName = "BattleLoot/Buff/Character/CritChance", order = 1)]
public class CritChanceBattleLoot : BuffBattleLoot
{
    [Header("Stats")]
    [SerializeField] private float critChanceAmount = 0.05f;
    private float CritChanceAmount {

        get => critChanceAmount + (critChanceAmount * CurrentRarityModifier);
    }

    public override string Description => description + " " + CritChanceAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.CritChance += CritChanceAmount;
    }

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.CritChance);
        afterUpgradeValue = CalculateString(playerData.CritChance + CritChanceAmount);
    }


}
