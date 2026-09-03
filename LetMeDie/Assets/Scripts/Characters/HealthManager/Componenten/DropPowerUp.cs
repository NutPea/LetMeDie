using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DropPowerUp : MonoBehaviour
{
    private HealthManager healthManager;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> powerUps;
    [SerializeField] private float powerUpProperbility = 0.01f;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(TryDropPowerUp);
    }

    private void TryDropPowerUp(GameObject arg0)
    {
        float percentage = UnityEngine.Random.Range(0.0f,1.0f);
        if (percentage < powerUpProperbility) {

            GameObject randomPowerUp = Instantiate(powerUps[UnityEngine.Random.Range(0, powerUps.Count - 1)]);
            randomPowerUp.transform.position = spawnPoint.position;

        }
    }

}
