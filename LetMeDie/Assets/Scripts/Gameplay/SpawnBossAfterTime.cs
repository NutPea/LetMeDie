using System;
using UnityEngine;

public class SpawnBossAfterTime : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPosition;
    void Start()
    {
        SGameManager.Instance.OnGameEnded.AddListener(SpawnBoss);
    }

    private void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab);
        boss.transform.position = spawnPosition.transform.position;

        if(boss.TryGetComponent(out BaseEnemyMovement baseEnemyMovement))
        {
            baseEnemyMovement.OnAggro();
        }
    }

  
}
