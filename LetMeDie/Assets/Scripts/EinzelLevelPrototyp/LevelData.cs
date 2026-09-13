using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelData",
    menuName = "Game/Level Data"
)]
public class LevelData : ScriptableObject
{



    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> startEnemyPrefabs;
    public List<GameObject> StartEnemyPrefabs => startEnemyPrefabs;
    [SerializeField] private List<GameObject> enemyPrefabs;


    [SerializeField] private int totalEnemiesToSpawn = 10;

    public int TotalEnemiesToSpawn => totalEnemiesToSpawn;

    public GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
            return null;

        return enemyPrefabs[
            Random.Range(0, enemyPrefabs.Count)
        ];
    }

}