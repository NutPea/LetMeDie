using System;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class DropEXPOrbsOnDeathHealthManagerExpansion : MonoBehaviour
{
    [SerializeField] private GameObject expDrop;
    [SerializeField] private int minAmountOfDrops = 1;
    [SerializeField] private int maxAmountOfDrops = 3;
    private HealthManager healthManager;

    [SerializeField] private float randomSpawnRadius = 0.75f;
    [SerializeField] private float dropPercentage = 0.5f;

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(SpawnDrops);
    }

    private void SpawnDrops(GameObject arg0)
    {
        float randomAmount = UnityEngine.Random.Range(0.0f, 1.0f);
        if(randomAmount > dropPercentage)
        {
            return;
        }


        int RandomAmountOfSpawns = UnityEngine.Random.Range(minAmountOfDrops,maxAmountOfDrops);
        for (int i = 0; i < RandomAmountOfSpawns; ++i) {
            GameObject drop = Instantiate(expDrop);
            drop.transform.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        return transform.position + new Vector3(UnityEngine.Random.Range(-randomSpawnRadius, randomSpawnRadius), 0, UnityEngine.Random.Range(-randomSpawnRadius, randomSpawnRadius));
    }
}
