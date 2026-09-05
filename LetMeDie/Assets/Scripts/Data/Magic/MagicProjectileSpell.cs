using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Magic/MagicProjectileSpell", order = 1)]
public class MagicProjectileSpell : MagicSpell
{
    [SerializeField] private int baseDamage = 5;
    public int BaseDamage => baseDamage;

    [HideInInspector]public int ExtraDamage = 0;

    private int CombineDamage => baseDamage + ExtraDamage;
    public int Damage => Mathf.CeilToInt((CombineDamage + (float)CombineDamage * playerData.SpellBaseDamagePercentage) * playerData.GetCritModifier());


    [SerializeField] private GameObject projectile;


    public override void Cast(Transform camera)
    {
        base.Cast(camera);
        SpawnProjectile(camera, camera.transform.position + camera.transform.forward,camera.transform.forward);
    }

    protected void SpawnProjectile(Transform camera,Vector3 spawnPosition, Vector3 lookDirection)
    {
        GameObject projectileSpell = Instantiate(projectile, camera.transform.position + camera.transform.forward, Quaternion.identity);
        ProjectileHandler projectileHandler = projectileSpell.GetComponent<ProjectileHandler>();
        projectileHandler.Init(Damage, 1, lookDirection, TeamFlag.Player);

        Vector3 size = projectileSpell.transform.localScale;
        projectileSpell.transform.localScale = size + size * playerData.ExtraAttackSize;
    }
}
