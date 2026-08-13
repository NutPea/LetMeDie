using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new();
    [SerializeField] private List<Transform> spawnPositions = new();
    [SerializeField] private int amountToSpawn = 10;

    public void Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++) { 
            GameObject toSpawnEnemy =  Instantiate(enemyPrefabs[Random.Range(0,enemyPrefabs.Count)]);
            Transform randomSpawn = spawnPositions[Random.Range(0,spawnPositions.Count)];
            toSpawnEnemy.transform.position = randomSpawn.position;
            if(toSpawnEnemy.TryGetComponent(out BaseEnemyMovement baseEnemyMovement))
            {
                baseEnemyMovement.OnAggro();
            }
            toSpawnEnemy.transform.forward = randomSpawn.forward;
        }
    }
}
