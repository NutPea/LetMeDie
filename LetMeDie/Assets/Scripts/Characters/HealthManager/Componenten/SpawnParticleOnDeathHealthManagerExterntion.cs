using System;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
public class SpawnParticleOnDeathHealthManagerExterntion : MonoBehaviour
{
    [SerializeField] private GameObject spawnParticle;
    private HealthManager healthManager;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath.AddListener(OnIsDestroy);
    }

    private void OnIsDestroy(GameObject diedObject)
    {
        GameObject particle = GameObject.Instantiate(spawnParticle,transform.position,Quaternion.identity);
        Destroy(particle, 1f);
    }


}
