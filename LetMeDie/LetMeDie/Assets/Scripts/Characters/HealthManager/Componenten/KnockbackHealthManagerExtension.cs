using System;
using UnityEngine;

public class KnockbackHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;
    private Rigidbody rb;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDamaged.AddListener(GetKnockbacked);
        rb = GetComponent<Rigidbody>();
    }

    private void GetKnockbacked(bool arg0, int arg1, float knockBack, Transform arg2)
    {
        Vector3 direction = transform.position - arg2.position;
        rb.AddForce(direction * knockBack, ForceMode.Impulse);
    }

}
