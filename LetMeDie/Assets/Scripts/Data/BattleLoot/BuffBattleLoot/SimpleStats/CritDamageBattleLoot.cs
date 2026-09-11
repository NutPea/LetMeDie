using UnityEngine;
[CreateAssetMenu(fileName = " CritDamage", menuName = "BattleLoot/Buff/CritDamage", order = 1)]
public class CritDamageBattleLoot : BuffBattleLoot
{
    [Header("Stats")]
    [SerializeField] private float critDamageAmount = 0.05f;
    private float CritDamageAmount => critDamageAmount + ( critDamageAmount * CurrentRarityModifier);

    public override string Description => description + " " + CritDamageAmount * 100f + "%";



    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraCritDamage += CritDamageAmount;
    }

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.ExtraCritDamage);
        afterUpgradeValue = CalculateString(playerData.ExtraCritDamage + CritDamageAmount);
    }
}
