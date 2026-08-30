using System;
using UnityEngine;

public class ProjektileExplosionHandler : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float knockBackStrength = 2f;
    [SerializeField] private float knockBackLength = 0.5f;
    private float ExplosionRadius => explosionRadius * transform.localScale.x;

    private ProjectileHandler projectileHandler;
    [SerializeField] private GameObject vfx;

    void Start()
    {
        projectileHandler = GetComponent<ProjectileHandler>();
        projectileHandler.OnDestroyAfterDurability.AddListener(Explode);
    }

    private void Explode()
    {
        Collider[] enemyCols = Physics.OverlapSphere(transform.position, ExplosionRadius);
        Debug.Log(enemyCols.Length);
        foreach (Collider col in enemyCols) {
            if (col.TryGetComponent(out HealthManager healthManager))
            {
                healthManager.InflictDamage(projectileHandler.Damage,TeamFlag.Player, transform);
                if(col.TryGetComponent(out BaseEnemyMovement baseEnemyMovement))
                {
                    Vector3 dir = healthManager.transform.position - transform.position;
                    dir = dir.normalized;
                    baseEnemyMovement.Knockback(dir * knockBackStrength, knockBackLength);
                }
            }

        }
        GameObject spawned = Instantiate(vfx);
        spawned.transform.position = transform.position;
        Destroy(spawned,0.5f);
    }


}
