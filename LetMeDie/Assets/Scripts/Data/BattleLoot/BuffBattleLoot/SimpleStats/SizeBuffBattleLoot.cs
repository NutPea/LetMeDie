using Unity.VisualScripting;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;

[CreateAssetMenu(fileName = "RageBuff", menuName = "BattleLoot/Buff/Size", order = 1)]
public class SizeBuffBattleLoot : BuffBattleLoot
{
    [SerializeField] private float extraSize = 0.05f;

    public override string Description => description + " " + extraSize * 100f + "%";

    public override void BuffBattleLootAdded(GameObject player, PlayerData data)
    {
        base.BuffBattleLootAdded(player, data);
        playerData.ExtraAttackSize += extraSize;
    }
}
