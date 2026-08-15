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

    internal BattleLoot GetBattleLoot()
    {
        return AvailableLoots[Random.Range(0,AvailableLoots.Count-1)];
    }
}
