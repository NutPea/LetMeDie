using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Bow", order = 1)]
public class BowData : WeaponData
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private bool canSpawnMorePrefabs;

    [SerializeField] private float spreadRadius = 0.5f;
    private int ExtraProjectiles => Mathf.CeilToInt(playerData.ExtraAmountOfProjectiles + (playerData.ExtraAmountOfProjectiles * playerData.ExtraAmountOfProjectilesPercent));

    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack( camera, chargeAmount);
        SpawnProjectile(camera, camera.transform.position, camera.transform.forward, chargeAmount);

        if (canSpawnMorePrefabs)
        {
            int rightAmount = Mathf.CeilToInt((float)ExtraProjectiles / 2);
            int leftAmount = ExtraProjectiles - rightAmount;


            float rightStep = spreadRadius / rightAmount;
            for (int x = 0; x < rightAmount; x++)
            {
                Vector3 newProjectileLookDirection = camera.transform.right * rightStep * (x + 1) + camera.transform.forward;
                newProjectileLookDirection = newProjectileLookDirection.normalized;
                SpawnProjectile(camera, camera.transform.position, newProjectileLookDirection,chargeAmount);
            }

            for (int y = 0; y < leftAmount; y++)
            {
                Vector3 newProjectileLookDirection = camera.transform.right * -rightStep * (y + 1) + camera.transform.forward;
                newProjectileLookDirection = newProjectileLookDirection.normalized;
                SpawnProjectile(camera, camera.transform.position, newProjectileLookDirection,chargeAmount);
            }
        }
    }

    private void SpawnProjectile(Transform camera, Vector3 position, Vector3 newProjectileLookDirection,float chargeAmount)
    {
        GameObject arrow = Instantiate(arrowPrefab, camera.transform.position + camera.transform.forward, Quaternion.identity);
        ProjectileHandler projectileHandler = arrow.GetComponent<ProjectileHandler>();
 
        Vector3 arrowScale = arrow.transform.localScale;
        arrow.transform.localScale = arrowScale + arrowScale * playerData.ExtraAttackSize;

        float calculatedChargeValue = PlayerData.CalculateChargeDamage(minDamageAmount, maxDamageAmount, chargeAmount);
        int calculatedDamage = Mathf.CeilToInt((calculatedChargeValue + (calculatedChargeValue * playerData.WeaponBaseDamagePercentage)) * playerData.GetCritModifier());
        projectileHandler.Init(calculatedDamage, chargeAmount, newProjectileLookDirection, TeamFlag.Player);
    }
}
