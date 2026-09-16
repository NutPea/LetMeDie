using System;
using System.Collections.Generic;
using UnityEngine;
using static SGameProgressionManager;

public class BossLevelManager : MonoBehaviour
{

    [SerializeField] private HealthManager boss;

    private List<GameProgressionDoor> gameProgressionDoors = new();

    void Start()
    {
        boss.OnDeath.AddListener(HandleBossDeath);


        gameProgressionDoors = new List<GameProgressionDoor>(
           FindObjectsByType<GameProgressionDoor>(
               FindObjectsInactive.Include,
               FindObjectsSortMode.None
           )
        );

        for (int i = 0; i < gameProgressionDoors.Count; ++i)
        {

            gameProgressionDoors[i].Init(RewardTyp.None);
        }
    }

    private void HandleBossDeath(GameObject boss)
    {
        foreach (GameProgressionDoor door in gameProgressionDoors)
        {
            door.ShowDoors();
        }
    }

}
