using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Bow", order = 1)]
public class BowData : WeaponData
{
    [SerializeField] private GameObject arrowPrefab;
    public override void Attack(Transform camera, float chargeAmount)
    {
        base.Attack( camera, chargeAmount);
        Debug.Log("Attack");
        GameObject arrow = Instantiate(arrowPrefab,camera.transform.position + camera.transform.forward,Quaternion.identity);
        ProjectileHandler projectileHandler = arrow.GetComponent<ProjectileHandler>();
        projectileHandler.Init(PlayerData.CalculateChargeDamage(minDamageAmount,maxDamageAmount,chargeAmount),chargeAmount, camera.transform.forward, TeamFlag.Player);

    }

}
