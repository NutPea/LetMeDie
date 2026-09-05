using UnityEngine;


[CreateAssetMenu(fileName = "AttackSpeedBuff", menuName = "BattleLoot/Buff/AttackSpeedBuff", order = 1)]
public class AttackSpeedBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float attackSpeedBuff = 0.05f;
    private float AttackSpeedBuff => attackSpeedBuff * CurrentRarityModifier;

    public override string Description => description + " " + AttackSpeedBuff * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraAttackSpeed += AttackSpeedBuff;
    }
}
