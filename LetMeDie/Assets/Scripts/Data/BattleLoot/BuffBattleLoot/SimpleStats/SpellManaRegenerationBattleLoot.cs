using UnityEngine;



[CreateAssetMenu(fileName = "SpellManaRegeneration", menuName = "BattleLoot/Buff/SpellManaRegeneration", order = 1)]
public class SpellManaRegenerationBattleLoot : BuffBattleLoot
{
    [SerializeField] private float spellManaReg = 0.05f;

    public override string Description => description + " " + spellManaReg;

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaRegeneration += spellManaReg;
    }


    public override void BuffBattleLootRemoved(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.SpellManaRegeneration -= spellManaReg;
    }
}
