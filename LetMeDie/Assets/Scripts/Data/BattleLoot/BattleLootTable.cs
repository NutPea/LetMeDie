using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "BattleLootTable", menuName = "BattleLoot/BattleLootTable", order = 1)]
public class BattleLootTable : ScriptableObject
{
    [SerializeField] private List<BattleLoot> battleLoots = new();

    public List<BattleLoot> AvailableLoots = new();

    public void Init()
    {
        AvailableLoots.Clear();
        foreach (BattleLoot battleLoot in battleLoots) {
            BattleLoot copiedLoot = Instantiate(battleLoot);
            AvailableLoots.Add(copiedLoot);
        }
    }

    public BattleLoot ChooseBattleLoot(BattleLoot battleLoot)
    {
        AvailableLoots.Remove(battleLoot);
        return battleLoot;
    }

    public void FillRarityTables(List<BattleLoot> commonTable, List<BattleLoot> uncommon, List<BattleLoot> rare, List<BattleLoot> epic, List<BattleLoot> legendary)
    {
        commonTable.Clear();
        uncommon.Clear();
        rare.Clear();
        epic.Clear();
        legendary.Clear();

        foreach (BattleLoot loot in battleLoots)
        {
            switch (loot.lootRarity)
            {
                case BattleLoot.LootRarity.Common: commonTable.Add(loot); break;
                case BattleLoot.LootRarity.Uncommen: uncommon.Add(loot); break;
                case BattleLoot.LootRarity.Rare: rare.Add(loot); break;
                case BattleLoot.LootRarity.Epic: epic.Add(loot); break;
                case BattleLoot.LootRarity.Legendary: legendary.Add(loot); break;

            }
        }

    }
    private const float UNCOMMON_DROP_MODIFIER = 0.06f;
    private const float RARE_DROP_MODIFIER = 0.06f;
    private const float EPIC_DROP_MODIFIER = 0.05f;
    private const float LEGENDARY_DROP_MODIFIER = 0.01f;

    public static BattleLoot.LootRarity GetRarity(int luck)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);

        if(randomValue < 0.01 + luck * LEGENDARY_DROP_MODIFIER)
        {
            return BattleLoot.LootRarity.Legendary;
        }

        if(randomValue< 0.05 + luck * EPIC_DROP_MODIFIER)
        {
            return BattleLoot.LootRarity.Epic;
        }


        if (randomValue < 0.2 + luck * RARE_DROP_MODIFIER)
        {
            return BattleLoot.LootRarity.Rare;
        }


        if (randomValue < 0.3 + luck * UNCOMMON_DROP_MODIFIER)
        {
            return BattleLoot.LootRarity.Uncommen;
        }



        return BattleLoot.LootRarity.Common;
    }



}
