using System;
using UnityEngine;

[RequireComponent (typeof(LevelManager))]
public class StartGameLevelManagerExtension : MonoBehaviour
{
    private LevelManager levelManager;
    private StartDoor startDoor;
    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        startDoor = FindAnyObjectByType<StartDoor>();
        startDoor.OnPlayerIsReady.AddListener(StartSpawningEnemies);
        startDoor.MovePlayer();
    }

    private void StartSpawningEnemies()
    {
        levelManager.StartHandlingEnemies();
    }
}
