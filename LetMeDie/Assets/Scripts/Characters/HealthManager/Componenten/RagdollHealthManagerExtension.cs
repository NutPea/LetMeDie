using Gameplay;
using System;
using UnityEngine;

[RequireComponent (typeof(HealthManager))]
[RequireComponent(typeof(RagdollHandler))]
public class RagdollHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;
    private RagdollHandler ragdollHandler;
    [SerializeField] private float timeUntilDestroy = 2f;
    [SerializeField] private Vector3 direction;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        ragdollHandler = GetComponent<RagdollHandler>();
        healthManager.OnDamaged.AddListener(LastTimeDamage);
        healthManager.OnDeath.AddListener(EnableRagdoll);
    }

    private void LastTimeDamage(bool arg0, int arg1, Transform player)
    {
        direction = player.transform.position - transform.position;
        direction = direction.normalized;
    }

    private void EnableRagdoll(GameObject diedObject)
    {
        ragdollHandler.EnableRagdoll();
        ragdollHandler.knockBackDirection = direction;
        Destroy(gameObject, timeUntilDestroy);
        Destroy(ragdollHandler.rootObject,timeUntilDestroy);
    }

}
