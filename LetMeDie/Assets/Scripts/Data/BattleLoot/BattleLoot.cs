using NUnit.Framework;
using UnityEngine;

public class BattleLoot : ScriptableObject
{
    [SerializeField] private string name;
    public virtual string Name => name;

    [SerializeField] protected string description;
    public virtual string Description => description;

    [SerializeField] private Sprite icon;
    public virtual Sprite Icon => icon;
    [SerializeField] Color tint = Color.white;

    public virtual Color Tint => tint;

    public enum LootRarity
    {
        Common,
        Uncommen,
        Rare,
        Epic,
        Legendary,
    }

    public LootRarity lootRarity;

    public virtual void CalculateValues()
    {
        
    }

}
