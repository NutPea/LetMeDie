using UnityEngine;

[CreateAssetMenu(fileName = " EvasionBuff", menuName = "BattleLoot/Buff/EvasionBuffBattleLoot", order = 1)]
public class EvasionBuffBattleLoot : BuffBattleLoot
{

    [Header("Stats")]
    [SerializeField] private float evasionPercentage = 0.2f;
    private float EvasionPercentage => evasionPercentage + (evasionPercentage * CurrentRarityModifier);
    public override string Description => description + (evasionPercentage * 100) +"%";


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.Evasion += evasionPercentage;

    }


    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.Evasion);
        afterUpgradeValue = CalculateString(playerData.Evasion + EvasionPercentage);
    }
}
