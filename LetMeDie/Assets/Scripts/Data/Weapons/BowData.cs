using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Bow", order = 1)]
public class BowData : WeaponData
{
    [SerializeField] private GameObject arrowPrefab;
    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack( camera, chargeAmount);
        GameObject arrow = Instantiate(arrowPrefab,camera.transform.position + camera.transform.forward,Quaternion.identity);
        ProjectileHandler projectileHandler = arrow.GetComponent<ProjectileHandler>();

        int calculatedDamage = Mathf.CeilToInt((PlayerData.CalculateChargeDamage(minDamageAmount, maxDamageAmount, chargeAmount) *playerData.WeaponBaseDamagePercentage) * playerData.GetCritModifier());
        projectileHandler.Init(calculatedDamage ,chargeAmount, camera.transform.forward, TeamFlag.Player);

    }

}
