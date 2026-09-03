using UnityEngine;
[CreateAssetMenu(fileName = " CritDamage", menuName = "BattleLoot/Buff/ CritDamage", order = 1)]
public class CritDamageBattleLoot : BuffBattleLoot
{
    [SerializeField] private float critDamageAmount = 0.05f;

    public override string Description => description + " " + critDamageAmount * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraCritDamage += critDamageAmount;
    }
}
