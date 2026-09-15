using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{


    [Header("Level")]
    [SerializeField] private LevelData levelData;

    [Header("Spawn Settings")]
    [SerializeField] private Transform defaultSpawnParent;

    // Alle aktuell lebenden Gegner
    private readonly HashSet<BaseEnemyController> registeredEnemies = new();

    // Wie viele Gegner aus dem LevelData noch gespawnt werden müssen
    private int remainingEnemiesToSpawn;

    private bool levelCompleted;

    /// <summary>
    /// Wird ausgelöst, wenn alle Gegner des Levels getötet wurden.
    /// </summary>
    [HideInInspector]public UnityEvent OnLevelCompleted;

    /// <summary>
    /// Anzahl aktuell lebender Gegner.
    /// </summary>
    public int AliveEnemyCount => registeredEnemies.Count;

    /// <summary>
    /// Anzahl Gegner, die noch gespawnt werden müssen.
    /// </summary>

    [SerializeField] private float radius = 50f;

    private void Awake()
    {
        RegisterExistingEnemies();
    }

    public void StartHandlingEnemies()
    {
        foreach(GameObject prefab in levelData.StartEnemyPrefabs)
        {
            SpawnEnemy(prefab);
        }
        CheckLevelCompleted();
        remainingEnemiesToSpawn = levelData.TotalEnemiesToSpawn;
    }


    private void OnDestroy()
    {
        foreach (BaseEnemyController enemy in registeredEnemies)
        {
            UnsubscribeFromEnemy(enemy);
        }

        registeredEnemies.Clear();
    }

    #region Enemy Registration

    private void RegisterExistingEnemies()
    {
        BaseEnemyController[] enemies =
            FindObjectsByType<BaseEnemyController>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (BaseEnemyController enemy in enemies)
        {
            RegisterEnemy(enemy);
        }
    }

    public void RegisterEnemy(BaseEnemyController enemy)
    {
        if (enemy == null)
            return;

        if (registeredEnemies.Contains(enemy))
            return;

        registeredEnemies.Add(enemy);

        SubscribeToEnemy(enemy);
    }

    public void UnregisterEnemy(BaseEnemyController enemy)
    {
        if (enemy == null)
            return;

        if (!registeredEnemies.Remove(enemy))
            return;

        UnsubscribeFromEnemy(enemy);

        CheckLevelCompleted();
    }

    private void SubscribeToEnemy(BaseEnemyController enemy)
    {
        if (enemy.HealthManager == null)
        {
            Debug.LogWarning(
                $"Enemy {enemy.name} besitzt keinen HealthManager.",
                enemy
            );

            return;
        }

        enemy.HealthManager.OnDeath.AddListener(HandleEnemyDeath);
    }

    private void UnsubscribeFromEnemy(BaseEnemyController enemy)
    {
        if (enemy == null || enemy.HealthManager == null)
            return;

        enemy.HealthManager.OnDeath.RemoveListener(HandleEnemyDeath);
    }

    #endregion

    #region Enemy Death

    private void HandleEnemyDeath(GameObject healthManager)
    {
        if (healthManager == null)
            return;

        BaseEnemyController enemy =
            healthManager.GetComponent<BaseEnemyController>();

        if (enemy == null)
            return;

        registeredEnemies.Remove(enemy);

        UnsubscribeFromEnemy(enemy);

        HandleEnemyKilled();
    }

    private void HandleEnemyKilled()
    {
        // Falls noch Gegner aus dem LevelData gespawnt werden müssen,
        // kann hier ein neuer Gegner erzeugt werden.
        if (remainingEnemiesToSpawn > 0)
        {
            SpawnNextEnemy();
        }

        CheckLevelCompleted();
    }

    #endregion

    #region Spawning

    private void SpawnNextEnemy()
    {
        if (levelData == null)
            return;

        GameObject enemyPrefab = levelData.GetRandomEnemyPrefab();

        if (enemyPrefab == null)
        {
            Debug.LogWarning("LevelData konnte kein Enemy-Prefab liefern.");
            return;
        }

        bool flowControl = SpawnEnemy(enemyPrefab);
        if (!flowControl)
        {
            return;
        }
        remainingEnemiesToSpawn--;
        if(enemyPrefab.TryGetComponent(out BaseEnemyController enemyController))
        {
            enemyController.SetAggro();
        }

        if (enemyPrefab.TryGetComponent(out BaseEnemyMovement movementController))
        {
            movementController.OnAggro();
        }
    }

    private bool SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 position = FindRandomPosition();

        GameObject enemyObject = Instantiate(
            enemyPrefab,
            position,
            Quaternion.identity,
            defaultSpawnParent
        );

        BaseEnemyController spawnedEnemy =
            enemyObject.GetComponent<BaseEnemyController>();

        if (spawnedEnemy == null)
        {
            Debug.LogError(
                $"Das Prefab {enemyPrefab.name} besitzt keinen BaseEnemyController.",
                enemyPrefab
            );

            Destroy(enemyObject);
            return false;
        }


        RegisterEnemy(spawnedEnemy);
        return true;
    }

    private Vector3 FindRandomPosition()
    {
        NavMeshHit hit;
        int tryAmounts = 100;
        Vector3 finalPosition = Vector3.zero;
        for (int i = 0; i < tryAmounts; i++)
        {
            Vector3 randomDirection = transform.position + UnityEngine.Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                break;
            }
        }

        return finalPosition;
    }


    #endregion

    #region Level Completion

    private void CheckLevelCompleted()
    {
        if (levelCompleted)
            return;

        // Noch lebende Gegner vorhanden
        if (registeredEnemies.Count > 0)
            return;

        // Noch Gegner im LevelData vorhanden
        if (remainingEnemiesToSpawn > 0)
            return;

        levelCompleted = true;

        Debug.Log("Level abgeschlossen!");
        OnLevelCompleted?.Invoke();
    }

    #endregion
}