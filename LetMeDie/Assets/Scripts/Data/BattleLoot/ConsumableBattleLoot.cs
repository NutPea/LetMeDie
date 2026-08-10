using Unity.Burst.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableBattleLoot", menuName = "BattleLoot/ConsumableBattleLoot", order = 1)]
public class ConsumableBattleLoot : BattleLoot
{
    [SerializeField] private ConsumbaleData consumableData;
    public ConsumbaleData ConsumableData => consumableData;

    public override string Name => consumableData.ItemName;
    public override string Description => "Gives " + consumableData.ItemName;

    public override Sprite Icon => consumableData.Sprite;

    public override Color Tint => consumableData.Tint;

    [SerializeField] private int amount;
    public int Amount => amount;
}
