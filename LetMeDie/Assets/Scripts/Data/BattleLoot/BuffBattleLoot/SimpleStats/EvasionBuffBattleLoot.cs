using UnityEngine;

[CreateAssetMenu(fileName = " EvasionBuff", menuName = "BattleLoot/Buff/EvasionBuffBattleLoot", order = 1)]
public class EvasionBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float evasionPercentage = 0.2f;

    public override string Description => description + (evasionPercentage * 100) +"%";


    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        data.Evasion += evasionPercentage;

    }
}
