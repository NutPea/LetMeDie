using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleItemsUIHandler : MonoBehaviour
{
    [SerializeField] private List<BattleItemUI> battleItemUI = new();

    private void Start()
    {
        battleItemUI.ForEach((n) => n.gameObject.SetActive(false));
    }

    public void RedrawUI(PlayerData playerData)
    {
        var result = playerData.BuffBattleLoots
            .GroupBy(x => x.GetType())
            .Select(g => new
            {
                Count = g.Count(),
                Loot = g.First()
            })
            .ToList();

        battleItemUI.ForEach(n => n.gameObject.SetActive(false));

        int currentBattleItemUI = 0;

        foreach (var buffItem in result)
        {
            if (!buffItem.Loot.ShowsAsItem)
            {
                continue;
            }
            battleItemUI[currentBattleItemUI].gameObject.SetActive(true);
            battleItemUI[currentBattleItemUI].SetUp(buffItem.Count, buffItem.Loot);

            currentBattleItemUI++;
        }
    }

}
