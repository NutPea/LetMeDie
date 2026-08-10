using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BattleLootTable", menuName = "BattleLoot/BattleLootTable", order = 1)]
public class BattleLootTable : ScriptableObject
{

    public List<BattleLoot> commonLoots = new();
    public List<BattleLoot> uncommonLoots = new();
    public List<BattleLoot> rareLoots = new();
    public List<BattleLoot> epicLoots = new();
    public List<BattleLoot> legendaryLoots = new();


    public BattleLoot GetRandomLoot(BattleLoot.LootRarity lootRarity)
    {
        switch (lootRarity)
        {
            case BattleLoot.LootRarity.Common: return commonLoots[Random.Range(0, commonLoots.Count - 1)];
            case BattleLoot.LootRarity.Uncommen: return uncommonLoots[Random.Range(0, uncommonLoots.Count - 1)];
            case BattleLoot.LootRarity.Rare: return rareLoots[Random.Range(0, rareLoots.Count - 1)];
            case BattleLoot.LootRarity.Epic: return epicLoots[Random.Range(0, epicLoots.Count - 1)];
            case BattleLoot.LootRarity.Legendary: return legendaryLoots[Random.Range(0, legendaryLoots.Count - 1)];
        }
        return commonLoots[Random.Range(0, commonLoots.Count - 1)];
    }

}
