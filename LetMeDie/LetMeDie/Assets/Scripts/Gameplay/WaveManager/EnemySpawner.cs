using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private List<Transform> spawnPositions = new();
    [SerializeField] private int amountToSpawn = 10;


    public void Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++) {
            Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            GameObject spawnedEnemy = Instantiate(enemyToSpawn);
            spawnedEnemy.transform.position = spawnPosition.position;
            spawnedEnemy.transform.forward = spawnPosition.forward;

            if(spawnedEnemy.TryGetComponent<BaseEnemyController>(out BaseEnemyController baseEnemyController))
            {
                baseEnemyController.SetAggro();
            }
        
        }
    }
}
