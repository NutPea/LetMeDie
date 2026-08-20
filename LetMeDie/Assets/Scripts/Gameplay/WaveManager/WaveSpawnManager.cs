using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawnManager : MonoBehaviour
{

    private SGameManager gameManager;
    [Header("Enemies")]
    [SerializeField] private List<SpawnEnemy> enemies = new();
    [SerializeField] private List<SpawnEnemy> afterGameTimeEnemies = new();


    [Header("Spawn Distance")]
    [SerializeField] private float cantSpawnDistance = 5.0f;
    [SerializeField] private float minSpawnDistance = 10.0f;
    [SerializeField] private float maxSpawnDistance = 30.0f;

    [SerializeField] private int spawnPositionAttempts = 20;


    [Header("Spawn Rate")]
    [SerializeField] private float startSpawnRate = 3.0f;
    [SerializeField] private float endSpawnRate = 0.5f;
    [SerializeField] private AnimationCurve spawnCurve;

    private float currentSpawnRate;


    [Header("Events")]
    [SerializeField] private List<SpawnEvent> events = new();

    private int nextEventIndex;

    private float spawnEventTimeRemaining;
    private float spawnEventMultiplier = 1.0f;


    private GameObject player;


    private void Start()
    {
        player = SGameManager.Instance.PlayerBody;

        currentSpawnRate = SpawnRate;

        // Make sure events are ordered by trigger time.
        events.Sort((a, b) =>
            a.TriggerTime.CompareTo(b.TriggerTime));

        gameManager = SGameManager.Instance;
    }


    private void Update()
    {

        UpdateEvents();

        UpdateSpawnEvent();

        UpdateSpawnTimer();
    }


    // ============================================================
    // EVENTS
    // ============================================================

    private void UpdateEvents()
    {
        if (nextEventIndex >= events.Count)
            return;

        SpawnEvent currentEvent = events[nextEventIndex];

        if (gameManager.ElapsedGameTime >= currentEvent.TriggerTime)
        {
            TriggerEvent(currentEvent);

            nextEventIndex++;
        }
    }


    private void TriggerEvent(SpawnEvent spawnEvent)
    {
        switch (spawnEvent.Type)
        {
            case SpawnEvent.EventType.Spawn:
                StartSpawnEvent(spawnEvent);
                break;

            case SpawnEvent.EventType.Boss:
                StartBossEvent(spawnEvent);
                break;
        }
    }


    // ============================================================
    // SPAWN EVENTS
    // ============================================================

    private void StartSpawnEvent(SpawnEvent spawnEvent)
    {
        spawnEventTimeRemaining = spawnEvent.Duration;

        spawnEventMultiplier =
            Mathf.Max(1.0f, spawnEvent.SpawnRateMultiplier);

        Debug.Log(
            $"Spawn Event Started: {spawnEvent.EventName}"
        );
    }


    private void UpdateSpawnEvent()
    {
        if (spawnEventTimeRemaining <= 0.0f)
        {
            spawnEventMultiplier = 1.0f;
            return;
        }

        spawnEventTimeRemaining -= Time.deltaTime;

        if (spawnEventTimeRemaining <= 0.0f)
        {
            spawnEventTimeRemaining = 0.0f;
            spawnEventMultiplier = 1.0f;

            Debug.Log("Spawn Event Finished");
        }
    }


    // ============================================================
    // BOSS EVENTS
    // ============================================================

    private void StartBossEvent(SpawnEvent spawnEvent)
    {
        if (spawnEvent.BossPrefab == null)
        {
            Debug.LogWarning(
                $"Boss event '{spawnEvent.EventName}' has no boss prefab."
            );

            return;
        }


        Debug.Log(
            $"Boss Event Started: {spawnEvent.EventName}"
        );


        for (int i = 0; i < spawnEvent.BossCount; i++)
        {
            if (TryGetSpawnPosition(out Vector3 spawnPosition))
            {
                Instantiate(
                    spawnEvent.BossPrefab,
                    spawnPosition,
                    Quaternion.identity
                );
            }
            else
            {
                Debug.LogWarning(
                    "Could not find a valid boss spawn position."
                );
            }
        }
    }


    // ============================================================
    // NORMAL SPAWNING
    // ============================================================

    private void UpdateSpawnTimer()
    {
        currentSpawnRate -= Time.deltaTime;

        if (currentSpawnRate <= 0.0f)
        {
            SpawnEnemy();

            currentSpawnRate = SpawnRate;
        }
    }


    private void SpawnEnemy()
    {
        if (player == null)
            return;


        List<SpawnEnemy> availableEnemies =
            gameManager.IsGameTime
                ? enemies
                : afterGameTimeEnemies;


        if (availableEnemies == null ||
            availableEnemies.Count == 0)
        {
            Debug.LogWarning(
                "No enemies available for spawning."
            );

            return;
        }


        SpawnEnemy enemy =
            GetWeightedEnemy(availableEnemies);


        if (enemy == null)
        {
            Debug.LogWarning(
                "Could not select an enemy."
            );

            return;
        }


        if (enemy.EnemyPrefab == null)
        {
            Debug.LogWarning(
                "Selected enemy has no prefab."
            );

            return;
        }


        if (!TryGetSpawnPosition(
                out Vector3 spawnPosition))
        {
            Debug.LogWarning(
                "Could not find a valid enemy spawn position."
            );

            return;
        }


        GameObject spawnedEnemy = Instantiate(
            enemy.EnemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        if(spawnedEnemy.TryGetComponent(out BaseEnemyMovement baseEnemyMovement)){
            baseEnemyMovement.OnAggro();
        }
    }


    // ============================================================
    // WEIGHTED ENEMY SELECTION
    // ============================================================

    private SpawnEnemy GetWeightedEnemy(
        List<SpawnEnemy> availableEnemies)
    {
        int totalWeight = 0;


        foreach (SpawnEnemy enemy in availableEnemies)
        {
            if (enemy == null)
                continue;


            int weight =
                GetCurrentWeight(enemy);


            totalWeight += weight;
        }


        if (totalWeight <= 0)
            return null;


        int randomValue =
            UnityEngine.Random.Range(
                0,
                totalWeight
            );


        foreach (SpawnEnemy enemy in availableEnemies)
        {
            if (enemy == null)
                continue;


            int weight =
                GetCurrentWeight(enemy);


            if (randomValue < weight)
                return enemy;


            randomValue -= weight;
        }


        return null;
    }


    private int GetCurrentWeight(
        SpawnEnemy enemy)
    {
        /*
         * Enemies have:
         *
         * Start Time
         * Maximum Weight
         *
         * Before Start Time:
         *      Weight = 0
         *
         * After Start Time:
         *      Weight gradually increases
         *      until Maximum Weight.
         */


        if (gameManager.ElapsedGameTime < enemy.StartTime)
            return 0;


        float timeSinceStart =
            gameManager.ElapsedGameTime - enemy.StartTime;


        float weightProgress =
            Mathf.Clamp01(
                timeSinceStart /
                enemy.WeightRampDuration
            );


        return Mathf.RoundToInt(
            Mathf.Lerp(
                0.0f,
                enemy.Weight,
                weightProgress
            )
        );
    }


    // ============================================================
    // SPAWN RATE
    // ============================================================

    private float SpawnRate =>
        CalculateSpawnRate();


    private float CalculateSpawnRate()
    {
        float rate;


        if (gameManager.IsGameTime)
        {
            /*
             * 0 = beginning of game
             * 1 = end of game
             */


            float progress =
                gameManager.ElapsedGameTime /
                gameManager.GameDuration;


            progress =
                Mathf.Clamp01(progress);


            rate =
                Mathf.Lerp(
                    startSpawnRate,
                    endSpawnRate,
                   spawnCurve.Evaluate(progress)
                );
        }
        else
        {
            rate = endSpawnRate;
        }


        /*
         * Spawn event multiplier.
         *
         * Example:
         *
         * Normal rate = 2 seconds
         * Multiplier = 4
         *
         * Result = 0.5 seconds
         */


        rate /= spawnEventMultiplier;


        return Mathf.Max(
            0.05f,
            rate
        );
    }


    // ============================================================
    // NAVMESH SPAWNING
    // ============================================================

    private bool TryGetSpawnPosition(
        out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;


        if (player == null)
            return false;


        for (int i = 0;
             i < spawnPositionAttempts;
             i++)
        {
            /*
             * Random direction around player.
             */


            Vector2 randomDirection =
                UnityEngine.Random.insideUnitCircle
                    .normalized;


            /*
             * Random distance between
             * min and max spawn distance.
             */


            float distance =
                UnityEngine.Random.Range(
                    minSpawnDistance,
                    maxSpawnDistance
                );


            Vector3 randomPosition =
                player.transform.position +
                new Vector3(
                    randomDirection.x,
                    0.0f,
                    randomDirection.y
                ) * distance;


            /*
             * Find the nearest NavMesh position.
             */


            if (!NavMesh.SamplePosition(
                    randomPosition,
                    out NavMeshHit hit,
                    5.0f,
                    NavMesh.AllAreas))
            {
                continue;
            }


            /*
             * Make sure the resulting NavMesh
             * position isn't too close to player.
             */


            float distanceToPlayer =
                Vector3.Distance(
                    player.transform.position,
                    hit.position
                );


            if (distanceToPlayer <
                cantSpawnDistance)
            {
                continue;
            }


            spawnPosition =
                hit.position;


            return true;
        }


        return false;
    }
}


// ================================================================
// SPAWN ENEMY
// ================================================================

[Serializable]
public class SpawnEnemy
{
    [SerializeField]
    private GameObject enemyPrefab;


    [Header("Difficulty")]

    [Tooltip(
        "Maximum weight this enemy can reach."
    )]
    [SerializeField]
    private int weight = 100;


    [Tooltip(
        "Time in seconds before this enemy starts appearing."
    )]
    [SerializeField]
    private float startTime = 0.0f;


    [Tooltip(
        "How long it takes for the enemy to reach its maximum weight."
    )]
    [SerializeField]
    private float weightRampDuration = 60.0f;


    public GameObject EnemyPrefab =>
        enemyPrefab;


    public int Weight =>
        Mathf.Max(0, weight);


    public float StartTime =>
        Mathf.Max(0.0f, startTime);


    public float WeightRampDuration =>
        Mathf.Max(0.01f, weightRampDuration);
}


// ================================================================
// SPAWN EVENT
// ================================================================

[Serializable]
public class SpawnEvent
{
    public enum EventType
    {
        Spawn,
        Boss
    }


    [Header("Event")]

    [SerializeField]
    private string eventName;


    [Tooltip(
        "Time in seconds from the start of the game."
    )]
    [SerializeField]
    private float triggerTime;


    [SerializeField]
    private EventType eventType;


    [Header("Spawn Event")]

    [Tooltip(
        "How long the increased spawn rate lasts."
    )]
    [SerializeField]
    private float duration = 10.0f;


    [Tooltip(
        "How much faster enemies spawn."
    )]
    [SerializeField]
    private float spawnRateMultiplier = 2.0f;


    [Header("Boss Event")]

    [SerializeField]
    private GameObject bossPrefab;


    [SerializeField]
    private int bossCount = 1;


    public string EventName =>
        eventName;


    public float TriggerTime =>
        Mathf.Max(0.0f, triggerTime);


    public EventType Type =>
        eventType;


    public float Duration =>
        Mathf.Max(0.0f, duration);


    public float SpawnRateMultiplier =>
        Mathf.Max(1.0f, spawnRateMultiplier);


    public GameObject BossPrefab =>
        bossPrefab;


    public int BossCount =>
        Mathf.Max(1, bossCount);
}