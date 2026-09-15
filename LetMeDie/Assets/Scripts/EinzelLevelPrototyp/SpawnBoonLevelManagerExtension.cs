using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SGameProgressionManager;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent (typeof(LevelManager))]
public class SpawnBoonLevelManagerExtension : MonoBehaviour
{
    [SerializeField] private Transform boonSpawnPoint;
    private LevelManager levelManager;
    private List<GameProgressionDoor> gameProgressionDoors = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GetComponent<LevelManager> ();
        levelManager.OnLevelCompleted.AddListener(SpawnBoon);
        gameProgressionDoors = new List<GameProgressionDoor>(
            FindObjectsByType<GameProgressionDoor>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            )
        );

        int enumCount = Enum.GetValues(typeof(RewardTyp)).Length;
        if(gameProgressionDoors.Count > enumCount)
        {
            Debug.LogError("You have to many doors and way to less boon");
        }

        List<int> values = new List<int>();
        for (int i = 0; i < gameProgressionDoors.Count; ++i) {
            for (int x = 0; x < 1000; ++x) {
                int randomValue = UnityEngine.Random.Range(1,enumCount);
                if (values.Contains(randomValue))
                {
                    continue;
                }
                values.Add(randomValue);
            }
        }

        for (int i = 0; i < gameProgressionDoors.Count; ++i)
        {
            RewardTyp reward = (RewardTyp)values[i];
            gameProgressionDoors[i].Init(reward);
        }

    }

    private void SpawnBoon()
    {
        GameObject potentialBoon = SGameProgressionManager.Instance.GetBoonPrefab();

        if (potentialBoon != null) {
            GameObject boon = Instantiate(SGameProgressionManager.Instance.GetBoonPrefab());
            boon.transform.position = boonSpawnPoint.position;

            if(boon.TryGetComponent(out BaseBoonPowerUp baseBoonPowerUp))
            {
                baseBoonPowerUp.OnPickUp.AddListener(OpenPossibleDoors);
            }
        }
        else
        {
            OpenPossibleDoors();
        }
    }

    private void OpenPossibleDoors()
    {

        foreach (GameProgressionDoor door in gameProgressionDoors) {
            door.ShowDoors();
        }
    }
}
