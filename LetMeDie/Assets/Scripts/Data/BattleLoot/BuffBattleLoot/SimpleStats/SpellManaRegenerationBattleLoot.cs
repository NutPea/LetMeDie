using UnityEngine;



[CreateAssetMenu(fileName = "SpellManaRegeneration", menuName = "BattleLoot/Buff/SpellManaRegeneration", order = 1)]
public class SpellManaRegenerationBattleLoot : BuffBattleLoot
{
    [Header("Stats")]
    [SerializeField] private float spellManaReg = 0.05f;
    private float SpellManaReg => spellManaReg + (spellManaReg * CurrentRarityModifier);

    public override string Description => description + " " + spellManaReg;

    public override void CalculateValues(PlayerData playerData)
    {
        base.CalculateValues(playerData);
        beforeUpgradeValue = CalculateString(playerData.SpellManaRegeneration);
        afterUpgradeValue = CalculateString(playerData.SpellManaRegeneration + SpellManaReg);
    }

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaRegeneration += SpellManaReg;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaRegeneration -= SpellManaReg;
    }
}
