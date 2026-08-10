using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapons/Magic/MagicProjectileSpell", order = 1)]
public class MagicProjectileSpell : MagicSpell
{
    [SerializeField] private int damage = 5;
    [SerializeField] private GameObject projectile;

    public override void Cast(Transform camera)
    {
        base.Cast(camera);
        GameObject projectileSpell = Instantiate(projectile, camera.transform.position + camera.transform.forward, Quaternion.identity);
        ProjectileHandler projectileHandler = projectileSpell.GetComponent<ProjectileHandler>();
        projectileHandler.Init(damage, 1, camera.transform.forward, TeamFlag.Player);
    }
}
