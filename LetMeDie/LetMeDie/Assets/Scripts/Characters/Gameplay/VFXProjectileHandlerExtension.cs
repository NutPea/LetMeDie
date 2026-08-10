using System;
using UnityEngine;

[RequireComponent(typeof(ProjectileHandler))]
public class VFXProjectileHandlerExtension : MonoBehaviour
{
    ProjectileHandler handler;
    [SerializeField] private GameObject vfx;
    [SerializeField] private float destroyTime = 0.5f;
    private void Start()
    {
        handler = GetComponent<ProjectileHandler>();
        handler.OnCollided.AddListener(SpawnVFX);
    }

    private void SpawnVFX()
    {
        GameObject projectile = Instantiate(vfx,transform.position,Quaternion.identity);
        Destroy(projectile,destroyTime);
    }
}
