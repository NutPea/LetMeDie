using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<SpawnEnemy> enemies = new();
    [SerializeField] private List<SpawnEnemy> afterGameTimeEnemies = new();


    [Header("Spawn Distance")]


    [SerializeField] private float cantSpawnDistance = 5.0f;
    [SerializeField] private float minSpawnDistance = 10.0f;
    [SerializeField] private float maxSpawnDistance = 30.0f;

    [SerializeField] private int spawnPositionAttempts = 20;


    [Header("Spawn Rate")]

    [Tooltip("Ab welcher Zeit überhaupt Gegner spawnen dürfen.")]
    [SerializeField] private float enemySpawnStartTime = 0.0f;

    [Tooltip("Spawnzeiten abhängig von der vergangenen GameTime.")]
    [SerializeField] private List<SpawnRateStep> spawnRateSteps = new();


    private float currentSpawnRate;


    [Header("Events")]
    [SerializeField] private List<SpawnEvent> events = new();

    private int nextEventIndex;

    private float spawnEventTimeRemaining;
    private float spawnEventMultiplier = 1.0f;


    private GameObject player;

    private bool canSpawn = false;

    public void StartSpawning()
    {
        canSpawn = true;
    }

    private void Start()
    {

        player = SGameManager.Instance.PlayerBody;

        /*
         * Spawnrate-Liste nach Zeit sortieren.
         *
         * Beispiel:
         *
         * 120 Sekunden
         * 20 Sekunden
         * 60 Sekunden
         *
         * wird zu:
         *
         * 20
         * 60
         * 120
         */
        spawnRateSteps.Sort((a, b) =>
            a.TriggerTime.CompareTo(b.TriggerTime));


        currentSpawnRate = SpawnRate;


        // Make sure events are ordered by trigger time.
        events.Sort((a, b) =>
            a.TriggerTime.CompareTo(b.TriggerTime));
    }


    private void Update()
    {
        if (!canSpawn)
        {
            return;
        }

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


        SpawnEvent currentEvent =
            events[nextEventIndex];


        if (SGameManager.Instance.ElapsedGameTime >=
            currentEvent.TriggerTime)
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
        spawnEventTimeRemaining =
            spawnEvent.Duration;

        spawnEventMultiplier =
            Mathf.Max(
                1.0f,
                spawnEvent.SpawnRateMultiplier
            );


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


        spawnEventTimeRemaining -=
            Time.deltaTime;


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


        for (int i = 0;
             i < spawnEvent.BossCount;
             i++)
        {
            if (TryGetSpawnPosition(
                    out Vector3 spawnPosition))
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
        /*
         * Noch keine Gegner spawnen?
         *
         * Timer pausieren.
         */
        if (!CanSpawnEnemies())
        {
            currentSpawnRate = SpawnRate;
            return;
        }


        currentSpawnRate -=
            Time.deltaTime;


        if (currentSpawnRate <= 0.0f)
        {
            SpawnEnemy();

            currentSpawnRate =
                SpawnRate;
        }
    }


    private void SpawnEnemy()
    {
        if (player == null)
            return;


        if (!CanSpawnEnemies())
            return;


        List<SpawnEnemy> availableEnemies =
            SGameManager.Instance.IsGameTime
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
            GetWeightedEnemy(
                availableEnemies
            );


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


        GameObject spawnedEnemy =
            Instantiate(
                enemy.EnemyPrefab,
                spawnPosition,
                Quaternion.identity
            );


        if (spawnedEnemy.TryGetComponent(
                out BaseEnemyMovement baseEnemyMovement))
        {
            baseEnemyMovement.OnAggro();
        }
    }


    // ============================================================
    // ENEMY SPAWN START
    // ============================================================

    private bool CanSpawnEnemies()
    {
        /*
         * Nach der normalen GameTime
         * werden die afterGameTimeEnemies verwendet.
         */
        if (!SGameManager.Instance.IsGameTime)
            return true;


        return SGameManager.Instance.ElapsedGameTime >=
               enemySpawnStartTime;
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
         * Vor StartTime:
         * Weight = 0
         *
         * Danach:
         * Weight steigt linear bis Maximum Weight.
         */

        if (SGameManager.Instance.ElapsedGameTime <
            enemy.StartTime)
        {
            return 0;
        }


        float timeSinceStart =
            SGameManager.Instance.ElapsedGameTime -
            enemy.StartTime;


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
        /*
         * Nach der GameTime:
         * schnellste eingestellte Spawnrate verwenden.
         */
        if (!SGameManager.Instance.IsGameTime)
        {
            if (spawnRateSteps == null ||
                spawnRateSteps.Count == 0)
            {
                return 1.0f;
            }


            return spawnRateSteps[
                spawnRateSteps.Count - 1
            ].SpawnRate / spawnEventMultiplier;
        }


        float elapsedTime =
            SGameManager.Instance.ElapsedGameTime;


        /*
         * Falls keine SpawnRate-Einträge vorhanden sind.
         */
        if (spawnRateSteps == null ||
            spawnRateSteps.Count == 0)
        {
            return 1.0f;
        }


        /*
         * Standardmäßig den ersten Wert verwenden.
         */
        float selectedSpawnRate =
            spawnRateSteps[0].SpawnRate;


        /*
         * Wir suchen den letzten Eintrag,
         * dessen TriggerTime bereits erreicht wurde.
         *
         * Beispiel:
         *
         * 0s   -> 3.0
         * 20s  -> 2.0
         * 60s  -> 1.0
         * 120s -> 0.5
         *
         * Bei 75 Sekunden:
         * -> 1.0
         */

        for (int i = 0;
             i < spawnRateSteps.Count;
             i++)
        {
            SpawnRateStep step =
                spawnRateSteps[i];


            if (elapsedTime >= step.TriggerTime)
            {
                selectedSpawnRate =
                    step.SpawnRate;
            }
            else
            {
                /*
                 * Da die Liste sortiert ist,
                 * können wir hier abbrechen.
                 */
                break;
            }
        }


        /*
         * Spawn Event Multiplikator anwenden.
         *
         * Beispiel:
         *
         * SpawnRate = 2 Sekunden
         * Multiplier = 4
         *
         * Ergebnis = 0.5 Sekunden
         */

        selectedSpawnRate /=
            spawnEventMultiplier;


        return Mathf.Max(
            0.05f,
            selectedSpawnRate
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
             * Zufällige Richtung um den Spieler.
             */

            Vector2 randomDirection =
                UnityEngine.Random.insideUnitCircle
                    .normalized;


            /*
             * Zufällige Distanz.
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
             * Nächste NavMesh Position suchen.
             */

            if (!NavMesh.SamplePosition(
                    randomPosition,
                    out NavMeshHit hit,
                    1000.0f,
                    NavMesh.AllAreas))
            {
                continue;
            }


            /*
             * Sicherheitsabstand zum Spieler.
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
// SPAWN RATE STEP
// ================================================================

[Serializable]
public class SpawnRateStep
{
    [Tooltip(
        "Ab welcher GameTime diese Spawnrate verwendet wird."
    )]
    [SerializeField]
    private float triggerTime = 0.0f;


    [Tooltip(
        "Zeit in Sekunden zwischen zwei Enemy-Spawns."
    )]
    [SerializeField]
    private float spawnRate = 1.0f;


    public float TriggerTime =>
        Mathf.Max(0.0f, triggerTime);


    public float SpawnRate =>
        Mathf.Max(0.05f, spawnRate);
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