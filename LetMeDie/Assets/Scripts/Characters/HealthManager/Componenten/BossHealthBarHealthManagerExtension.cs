using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class BossHealthBarHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;



    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        SGameManager.Instance.RegisterBoss(healthManager);
    }

}
