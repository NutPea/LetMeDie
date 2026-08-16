using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Magic/MagicProjectileSpell", order = 1)]
public class MagicProjectileSpell : MagicSpell
{
    [SerializeField] private int baseDamage = 5;
    public int BaseDamage => baseDamage;

    [HideInInspector]public int ExtraDamage = 0;

    private int CombineDamage => baseDamage + ExtraDamage;
    public int Damage => CombineDamage + Mathf.CeilToInt((float)CombineDamage * playerData.SpellBaseDamagePercentage);


    [SerializeField] private GameObject projectile;

    public override void Cast(Transform camera)
    {
        base.Cast(camera);

        GameObject projectileSpell = Instantiate(projectile, camera.transform.position + camera.transform.forward, Quaternion.identity);
        ProjectileHandler projectileHandler = projectileSpell.GetComponent<ProjectileHandler>();
        projectileHandler.Init(Damage, 1, camera.transform.forward, TeamFlag.Player);
    }
}
