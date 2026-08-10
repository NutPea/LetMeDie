using Essentials;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class WaveHandler : MonoBehaviour
{
    public UnityEvent<int, Wave> OnWaveChange = new();
    public List<Wave> Waves;
    public int CurrentWaveIndex = 0;
    [SerializeField] private List<Transform> spawnPoints;
    public Wave CurrentWave => Waves[CurrentWaveIndex];
    private List<GameObject> spawnedEnemies = new();

    private bool everyEnemyHasBeenSpawned = false;

    [SerializeField] private GameObject player;
    public enum WaveState
    {
        Start,Spawn,WaitUntilEverythingIsDead,WaitTransition
    }

    [SerializeField] private float waveStartTime = 1f;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float waitTransitionTime = 0.5f;

    public WaveState currentWaveState = WaveState.Start;

    private void Start()
    {
        HandleWaveStart();
        player = SGameManager.Instance.PlayerBody;
        player.GetComponent<PlayerStatHandler>().PlayerData.OnLevelUp.AddListener(ChangeToLevelUp);
    }

    private void ChangeToLevelUp(int arg0)
    {
        SUIManager.Instance.ChangeToUIState(SUIManager.LEVEL_UP_UI_STATENAME);
    }

    private void HandleWaveStart()
    {
        currentWaveState = WaveState.Start;
        Invoke(nameof(HandleWaveSpawn), waveStartTime);
        everyEnemyHasBeenSpawned = false;
        OnWaveChange.Invoke(CurrentWaveIndex, CurrentWave);
    }

    private void HandleWaveSpawn()
    {
        CurrentWave.OnAllEnemysHaveBeenSpawned.AddListener(HandleWaitUntilEverythingIsDead);
        currentWaveState = WaveState.Spawn;
        SpawnEnemy();
    }

    private void HandleWaitUntilEverythingIsDead()
    {
        CancelInvoke(nameof(SpawnEnemy));
        CurrentWave.OnAllEnemysHaveBeenSpawned.RemoveListener(HandleWaitUntilEverythingIsDead);
        currentWaveState = WaveState.WaitUntilEverythingIsDead;
        everyEnemyHasBeenSpawned = true;
    }

    private void SpawnEnemy()
    {
        if (everyEnemyHasBeenSpawned)
        {
            return;
        }
        GameObject spawnedEnemy = Instantiate(CurrentWave.SpawnEnemy());
        HealthManager healthManager = spawnedEnemy.GetComponentInChildren<HealthManager>();
        healthManager.OnDeath.AddListener(RemoveFromSpawnedList);
        spawnedEnemies.Add(spawnedEnemy);

        int randomSpawnPoint = UnityEngine.Random.Range(0, spawnPoints.Count-1);
        Transform spawnPoint = spawnPoints[randomSpawnPoint];
        spawnedEnemy.transform.position = spawnPoint.transform.position;
        spawnedEnemy.transform.forward = spawnPoint.transform.forward;

        Invoke(nameof(SpawnEnemy), timeBetweenSpawns);

    }

    private void RemoveFromSpawnedList(GameObject diedEnemy)
    {
        if (diedEnemy != null) {
            spawnedEnemies.Remove(diedEnemy);
        }
        else{
            spawnedEnemies.RemoveAll(x => x == null);
        }
        if(currentWaveState == WaveState.WaitUntilEverythingIsDead){
            if(spawnedEnemies.Count == 0){
                HandleTransitionTime();
            }
        }
    }

    private void HandleTransitionTime()
    {
        CurrentWave.Reset();
        CurrentWaveIndex++;
        if (CurrentWaveIndex > Waves.Count-1) {
            CurrentWaveIndex = Waves.Count-1;
        }
        Invoke(nameof(HandleWaveStart), waitTransitionTime);
    }

}
