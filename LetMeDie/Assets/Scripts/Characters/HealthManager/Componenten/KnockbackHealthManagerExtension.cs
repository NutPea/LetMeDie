using System;
using UnityEngine;

public class KnockbackHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;
    private PlayerCharacterControllerMovementController playerCharacterControllerMovementController;
    private Rigidbody rb;
    [SerializeField] private float knockBackAmount = 5f;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDamaged.AddListener(GetKnockbacked);
        rb = GetComponent<Rigidbody>();
        playerCharacterControllerMovementController = GetComponent<PlayerCharacterControllerMovementController>();
    }

    private void GetKnockbacked(bool arg0, int arg1, Transform arg2)
    {
        Vector3 direction = transform.position - arg2.position;
        playerCharacterControllerMovementController.ApplyKnockback(direction, knockBackAmount, knockBackAmount);
    }

}
