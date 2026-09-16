using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SGameProgressionManager;

[RequireComponent (typeof(HealthManager))]
public class BossHealthBarHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;



    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        healthManager = GetComponent<HealthManager>();
        SGameManager.Instance.RegisterBoss(healthManager);
    }

}
