using System;
using UnityEngine;

public class KnockbackHealthManagerExtension : MonoBehaviour
{
    private HealthManager healthManager;
    private Rigidbody rb;
    [SerializeField] private float knockBackAmount = 5f;
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDamaged.AddListener(GetKnockbacked);
        rb = GetComponent<Rigidbody>();
    }

    private void GetKnockbacked(bool arg0, int arg1, Transform arg2)
    {
        Vector3 direction = transform.position - arg2.position;
        rb.AddForce(direction * knockBackAmount,ForceMode.Impulse);
    }

}
