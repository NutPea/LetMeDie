using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Wave
{

    public enum WaveType { Combat = 0,Loot = 1,Boss = 2}
    public WaveType waveType;

    [SerializeField] private List<WaveEnemy> enemies;
    [HideInInspector] public UnityEvent OnAllEnemysHaveBeenSpawned = new();

    private WaveEnemy CurrentEnemy => enemies[currentEnemySpawnIndex];
    private int currentEnemySpawnIndex = 0; 

    public GameObject SpawnEnemy()
    {
        GameObject spawnedEnemy = CurrentEnemy.Enemy;
        CurrentEnemy.CurrentSpawnedAmount++;

        if (CurrentEnemy.HasAllBeenSpawned)
        {
            currentEnemySpawnIndex++;
            if(currentEnemySpawnIndex >= enemies.Count)
            {
                OnAllEnemysHaveBeenSpawned.Invoke();
            }
        }
        return spawnedEnemy;
    }

    public void Reset()
    {
        currentEnemySpawnIndex = 0;
        CurrentEnemy.CurrentSpawnedAmount = 0;
    }
}


[System.Serializable]
public class WaveEnemy
{
    public GameObject Enemy;
    [SerializeField] private int amount;
    public int CurrentSpawnedAmount = 0;
    public bool HasAllBeenSpawned => CurrentSpawnedAmount >= amount;

}